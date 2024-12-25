using Microsoft.AspNetCore.Mvc;
using QLBanHang_UI.Models.Request;
using System.Text.Json;
using System.Text;
using QLBanHang_UI.Models;
using System.Net.Http;
using System.Security.Claims;

namespace QLBanHang_UI.Areas.User.Controllers
{
    [Area("User")]
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        public OrderController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        //View Order and OrderDetail
        public async Task<IActionResult> CheckOut(OrderFormRequest orderFormRequest)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart");
            var totalAmount = cart.Sum(x => x.Quantity * (x.Product?.PromotionPrice ?? x.Product?.Price));
            //GetUserId
            var userId= new Guid();
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            }
            var promotion = new PromotionDto();
            //Create Order
            var order = new OrderRequest()
            {
                OrderId = Guid.NewGuid(),
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount ?? 0,
                Status = "Pending",
                UserId = userId,
            };
            if (!string.IsNullOrEmpty(orderFormRequest.PromotionCode))
            {
                promotion = await GetPromotion(orderFormRequest.PromotionCode);
                if (promotion != null)
                {
                    if (promotion.EndDate > order.OrderDate && promotion.MaxUsage > 0 && order.OrderDate > promotion.StartDate)
                    {
                        order.TotalAmount = (decimal)totalAmount * (100 - promotion.Percentage) / 100;
                        var updatePromotion = new UpPromotionRequest()
                        {
                            PromotionId = promotion.PromotionId,
                            StartDate = promotion.StartDate,
                            EndDate = promotion.EndDate,
                            Code = promotion.Code,
                            MaxUsage = promotion.MaxUsage - 1,
                            Percentage = promotion.Percentage,
                        };
                        await UpdatePromotion(updatePromotion);
                        order.DiscountPercentage = promotion.Percentage;
                        order.PromotionId = promotion.PromotionId;
                    }
                }
                
            }
            //Create OrderDetail
            var orderDetails = new List<OrderDetailsRequest>();
            foreach (var item in cart)
            {
                var orderDetail = new OrderDetailsRequest()
                {
                    OrderDetailId = Guid.NewGuid(),
                    OrderId = order.OrderId,
                    UnitPrice = item.Product!.PromotionPrice ?? item.Product.Price,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                };
                orderDetails.Add(orderDetail);
            }
            var targetUrl = await CreateOrder(order);
            if (targetUrl != null)
            {
                var userInfo = await GetUserInfo(userId);
                if (userInfo != null)
                {
                    var updateUserInfo = new UpdateUserInfo()
                    {
                        FirstName = orderFormRequest.FirstName,
                        LastName = orderFormRequest.LastName,
                        Address = orderFormRequest.Address,
                        PhoneNumber = orderFormRequest.PhoneNumber,
                    };
                    var updateUserInfo2 = await UpdateUserInfo(userId, updateUserInfo);
                }
                else
                {
                    var addUserInfo = new AddUserInfo()
                    {
                        FirstName = orderFormRequest.FirstName,
                        LastName = orderFormRequest.LastName,
                        Address = orderFormRequest.Address,
                        PhoneNumber = orderFormRequest.PhoneNumber,
                        UserId = userId,
                    };
                    var addUserInfo2 = await AddUserInfo(addUserInfo);
                }
                var orderDetailsDto = await CreateOrderDetail(orderDetails);
            }
            
            return Redirect(targetUrl);
        }
        //Update Promotion
        public async Task UpdatePromotion(UpPromotionRequest upPromotionDto)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri("https://localhost:7069/api/Promotion/Update"),
                    Content = new StringContent(JsonSerializer.Serialize(upPromotionDto), Encoding.UTF8, "application/json")
                };
                var httpResponse = await client.SendAsync(httpMessage);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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
        public async Task<string> CreateOrder(OrderRequest orderRequest)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7069/api/Order/checkout?payment=VNP"),
                    Content = new StringContent(JsonSerializer.Serialize(orderRequest), Encoding.UTF8, "application/json")
                };
                var httpResponse = await client.SendAsync(httpMessage);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                var targetUrl = await httpResponse?.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(targetUrl))
                {
                    return string.Empty;
                }
                return targetUrl;
            }
            catch (Exception ex)
            {
                return null;
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
                    RequestUri = new Uri($"https://localhost:7069/api/Promotion/GetByCode/{code}"),
                };
                var httpResponse = await client.SendAsync(httpMessage);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                var promotion = await httpResponse?.Content.ReadFromJsonAsync<PromotionDto>() ?? new PromotionDto();
                return promotion;
            }
            catch (Exception ex)
            {
                return new PromotionDto();
            }
        }

        //Get UserInfo
        public async Task<UserInfoDto> GetUserInfo(Guid id)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7069/api/UserInfo/GetId/{id}"),
                };
                var httpResponse = await client.SendAsync(httpMessage);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                var userInfo = await httpResponse?.Content.ReadFromJsonAsync<UserInfoDto>() ?? new UserInfoDto();
                return userInfo;
            }
            catch (Exception ex)
            {
                return new UserInfoDto();
            }
        }

        //Add UserInfo
        public async Task<UserInfoDto> AddUserInfo(AddUserInfo addUserInfo)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://localhost:7069/api/UserInfo/Add"),
                    Content = new StringContent(JsonSerializer.Serialize(addUserInfo),Encoding.UTF8,"application/json")
                };
                var httpResponse = await client.SendAsync(httpMessage);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                var userInfo = await httpResponse?.Content.ReadFromJsonAsync<UserInfoDto>() ?? new UserInfoDto();
                return userInfo;
            }
            catch (Exception ex)
            {
                return new UserInfoDto();
            }
        }

        public async Task<UserInfoDto> UpdateUserInfo(Guid id, UpdateUserInfo updateUserInfo)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7069/api/UserInfo/Update/{id}"),
                    Content = new StringContent(JsonSerializer.Serialize(updateUserInfo), Encoding.UTF8, "application/json")
                };
                var httpResponse = await client.SendAsync(httpMessage);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                var userInfo = await httpResponse?.Content.ReadFromJsonAsync<UserInfoDto>() ?? new UserInfoDto();
                return userInfo;
            }
            catch (Exception ex)
            {
                return new UserInfoDto();
            }
        }
    }

    
}

