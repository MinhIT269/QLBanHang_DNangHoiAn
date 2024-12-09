using Microsoft.AspNetCore.Mvc;
using QLBanHang_UI.Models;
using QLBanHang_UI.Models.Request;
using QLBanHang_API.Dto.Request;
using System.Net;
using System.Text.Json;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Primitives;

namespace QLBanHang_UI.Areas.User.Controllers
{
	[Area("User")]
	public class CartController : Controller
    {
		private readonly IHttpClientFactory httpClientFactory;
		public CartController(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
		}
        
		// Xử lý View 
		//View Cart
		public IActionResult CartView()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart");
            ViewData["Carts"] = cart;
            return View("~/Areas/User/Views/Cart.cshtml");
        }

		//View Checkout
		[HttpGet]
		public IActionResult CheckoutView()
		{
			var cart = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart");
			ViewData["Carts"] = cart;
			return View("~/Areas/User/Views/Checkout.cshtml");
		}
		

		// View RemoveItem
        [HttpGet]
        public IActionResult RemoveItem(Guid id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart");
            var remove = cart.RemoveAll(x => x.CartItemId == id);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            if (remove != null)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
		}

		// View UpdateCart
		[HttpGet]
		public IActionResult UpdateCart(List<UpdateCartRequest> cart)
		{
			if (cart == null || !cart.Any())
			{
				return BadRequest(new { message = "Dữ liệu giỏ hàng không hợp lệ" });
			}
			var cartNew = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart");
			if (cart == null)
			{
				return BadRequest(new { message = "Giỏ hàng không tồn tại" });
			}
			foreach (var item in cart)
			{
				var updateItem = cartNew.FirstOrDefault(x => x.CartItemId == item.CartItemId);
				if (updateItem != null)
				{
					updateItem.Quantity = item.Quantity > 0 ? item.Quantity : 1; // Đảm bảo số lượng >= 1
				}
			}
			HttpContext.Session.SetObjectAsJson("Cart", cartNew);
			// Trả về JSON kết quả
			return RedirectToAction("CartView","Cart");
		}


		//View Order and OrderDetail 
		/*public async Task<IActionResult> CreateOrderView()
		{
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart");
			var promotion = await GetPromotion("");
            var totalAmount = cart.Sum(x => x.Quantity * (x.Product?.PromotionPrice ?? x.Product?.Price)) ;
			if (promotion.EndDate < DateTime.Now && promotion.MaxUsage > 0)
			{
                totalAmount = totalAmount - (totalAmount * promotion.Percentage / 100);
				var updatePromotion = new QLBanHang_API.Dto.UpPromotionDto()
				{
					PromotionId = promotion.PromotionId,
					StartDate = promotion.StartDate,
					EndDate = promotion.EndDate,
					Code = promotion.Code,
					MaxUsage = promotion.MaxUsage - 1,
					Percentage = promotion.Percentage,
				};
				await UpdatePromotion(updatePromotion);
            }
			var userId = Guid.Parse(Request.Cookies["UserId"]);
            var order = new OrderRequest()
			{
				OrderId = Guid.NewGuid(),
				OrderDate = DateTime.Now,
				TotalAmount = totalAmount ?? 0,
				Status = "Pending",
				UserId = userId,
				PromotionId = promotion.PromotionId,
				DiscountPercentage = promotion.Percentage,
			};

			var orderDetails = new List<OrderDetailsRequest>();
			foreach (var item in cart)
			{
				var orderDetail = new OrderDetailsRequest()
				{
					OrderDetailId  = Guid.NewGuid(),
					OrderId = order.OrderId,
					UnitPrice = item.Product.PromotionPrice ?? item.Product.Price,
					ProductId = item.ProductId,
					Quantity = item.Quantity,
				};
				orderDetails.Add(orderDetail);
			}

			var orderDto = await CreateOrder(order);
			var orderDetailsDto = await CreateOrderDetail(orderDetails);
		}*/



		// Xử lý Logic gọi API

		//Tạo OrderDetail
		public async Task<List<OrderDetailDto>> CreateOrderDetail(List<OrderDetailsRequest> orderDetailsRequests)
		{
			try
			{
				var client = httpClientFactory.CreateClient();
				var httpMessage = new HttpRequestMessage()
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri("https://localhost:7069/api/Order/CreateOrderDetail"),
					Content = new StringContent(JsonSerializer.Serialize(orderDetailsRequests), Encoding.UTF8, "application/json")
				};
				var httpResponse = await client.SendAsync(httpMessage);
				if (!httpResponse.IsSuccessStatusCode)
				{
					return null;
				}

				var response = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<OrderDetailDto>>();
				return response.ToList() ?? new List<OrderDetailDto>();
			}
			catch (Exception ex)
			{
				return new List<OrderDetailDto>();
			}
		}


		//Tạo Order
		public async Task<OrderDto> CreateOrder(OrderRequest orderRequest)
		{
			try
			{
				var client = httpClientFactory.CreateClient();
				var httpMessage = new HttpRequestMessage()
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri("https://localhost:7069/api/Order/CreateOrderDetail"),
					Content = new StringContent(JsonSerializer.Serialize(orderRequest), Encoding.UTF8, "application/json")
				};
				var httpResponse = await client.SendAsync(httpMessage);
				if (!httpResponse.IsSuccessStatusCode)
				{
					return null;
				}
				var order = await httpResponse?.Content.ReadFromJsonAsync<OrderDto>();
				return order;
			}
			catch(Exception ex)
			{
				return new OrderDto();
			}
		}

		//Get Promotion 
		public async Task<PromotionDto> GetPromotion(string code)
		{
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7069/api/Promotion/GetOne/{code}"),
                };
                var httpResponse = await client.SendAsync(httpMessage);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                var promotion = await httpResponse?.Content.ReadFromJsonAsync<PromotionDto>();
                return promotion;
            }
            catch (Exception ex)
            {
                return new PromotionDto();
            }
        }

		//Update Promotion
		public async Task UpdatePromotion(QLBanHang_API.Dto.UpPromotionDto upPromotionDto) 
		{
			try
			{
				var client = httpClientFactory.CreateClient();
				var httpMessage = new HttpRequestMessage() {
					Method = HttpMethod.Put,
					RequestUri = new Uri("https://localhost:7069/api/Promotion/Update"),
					Content = new StringContent(JsonSerializer.Serialize(upPromotionDto), Encoding.UTF8, "application/json")
				};
				var httpResponse = await client.SendAsync(httpMessage);

			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
