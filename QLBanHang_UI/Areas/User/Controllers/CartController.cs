using Microsoft.AspNetCore.Mvc;
using QLBanHang_UI.Models;
using System.Net;
using System.Text.Json;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Primitives;
using QLBanHang_UI.Models.Request;

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
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
			{
				var viewModel = HttpContext.Session.GetObjectFromJson<ViewModel>("ViewModel");
				viewModel.Carts = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart"); ;
                return View("~/Areas/User/Views/Cart.cshtml",viewModel);
            }
            return RedirectToAction("Login","Auth");
        }

		//View Checkout
		[HttpGet]
		public IActionResult CheckoutView()
		{
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var viewModel = HttpContext.Session.GetObjectFromJson<ViewModel>("ViewModel");
                viewModel.Carts = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart"); ;
                return View("~/Areas/User/Views/Checkout.cshtml",viewModel);
            }
            return RedirectToAction("Login", "Auth");
            
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
		
	}
}
