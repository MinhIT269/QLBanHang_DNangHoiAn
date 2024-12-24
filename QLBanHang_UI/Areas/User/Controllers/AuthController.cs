using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using QLBanHang_UI.Models.Response;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using QLBanHang_UI.Models.Request;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using QLBanHang_UI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
namespace QLBanHang_UI.Areas.User.Controllers
{
    [Area("User")]
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        public AuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Areas/User/Views/Authentication/Login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7069/api/Auth/Login"),
                    Content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json")
                };
                var httpResponse = await client.SendAsync(httpRequestMessage);

                if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", content);
                    return View("~/Areas/User/Views/Authentication/Login.cshtml");
                }

                var response = await httpResponse.Content.ReadFromJsonAsync<LoginResponse>();
                Console.WriteLine(response.JwtToken);
                if (response == null)
                {
                    ModelState.AddModelError("", "Login failed:");
                    return View("~/Areas/User/Views/Authentication/Login.cshtml");
                }
                var role = GetRoleFromToken(response.JwtToken);
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,response.UserId.ToString()),
                    new Claim("JwtToken",response.JwtToken),
                    new Claim(ClaimTypes.Role,role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
                await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));
                if(!string.IsNullOrEmpty(role) && role=="Admin"){
                    return RedirectToAction("Index","Admin", new { area="Admin"});
                }
                return RedirectToAction("DashBoard", "DashBoard");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
                ModelState.AddModelError("", "Có lỗi xảy ra khi đăng nhập: " + ex.Message);
                return View("~/Areas/User/Views/Authentication/Login.cshtml");
            }

        }

        // Logout 
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart");
            if (cart != null)
            {
                var cartUpdate = new List<CartItemRequest>();
                foreach (var item in cart)
                {
                    var cartItem = new CartItemRequest()
                    {
                        CartId = item.CartId,
                        CartItemId = item.CartItemId,
                        Quantity = item.Quantity,
                        ProductId = item.ProductId,
                    };
                    cartUpdate.Add(cartItem);
                }
                await UpdateCart(cartUpdate);
            }
            await HttpContext.SignOutAsync("MyCookieAuth");
            HttpContext.Session.Clear();
            var cartAfterClear = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart");
            if (cartAfterClear == null)
            {
                Console.WriteLine("Session đã được xóa.");
            }
            else
            {
                Console.WriteLine("Session chưa được xóa.");
            }
            return RedirectToAction("Login", "Auth");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Areas/User/Views/Authentication/Register.cshtml");  // Chắc chắn rằng đường dẫn view chính xác
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                // Nếu ModelState không hợp lệ, trả về view cùng dữ liệu người dùng đã nhập
                return View("~/Areas/User/Views/Authentication/Register.cshtml", registerRequest);
            }

            try
            {
                var client = httpClientFactory.CreateClient();
                var httpRequest = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7069/api/Auth/Register"),
                    Content = new StringContent(JsonSerializer.Serialize(registerRequest), Encoding.UTF8, "application/json")
                };

                var httpResponse = await client.SendAsync(httpRequest);
                var content = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    // Đọc nội dung lỗi từ phản hồi của API và hiển thị dưới dạng ModelState error

                    ViewData["BadRequest"] = content;

                    // Trả về view với lỗi từ ModelState
                    return View("~/Areas/User/Views/Authentication/Register.cshtml");
                }

                // Nếu đăng ký thành công, chuyển hướng tới trang đăng nhập
                return RedirectToAction("Login", "Auth", new { area = "User" });
            }
            catch (Exception ex)
            {
                // Thêm lỗi chung vào ModelState khi có ngoại lệ
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again later.");

                // Trả về view với lỗi ngoại lệ
                return View("~/Areas/User/Views/Authentication/Register.cshtml", registerRequest);
            }
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View("~/Areas/User/Views/Authentication/ResetPassword.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail(string Email)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7069/api/Auth/SendMail"),
                    Content = new StringContent(JsonSerializer.Serialize(Email), Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(httpRequest);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle different status codes here if needed
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        ModelState.AddModelError(string.Empty, "Email not found.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to send email.");
                    }
                    //Reload lại trang để hiển thị lỗi
                    return View("~/Areas/User/Views/Authentication/ResetPassword.cshtml");
                }

                var content = await response.Content.ReadAsStringAsync();
                ViewBag.Message = content;
                return View("~/Areas/User/Views/Authentication/ResetPassword.cshtml"); // You may use a specific view or pass content as a model if needed
            }
            catch (Exception ex)
            {
                // Log exception here if you have logging enabled
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View("~/Areas/User/Views/Authentication/ResetPassword.cshtml"); // Consider using an error view
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpRequest = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7069/api/Auth/ResetPassword"),
                    Content = new StringContent(JsonSerializer.Serialize(resetPasswordRequest), Encoding.UTF8, "application/json")
                };
                var httpResponse = await client.SendAsync(httpRequest);
                var content = await httpResponse.Content.ReadAsStringAsync();
                if (!httpResponse.IsSuccessStatusCode)
                {
                    if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
                    {
                        ModelState.AddModelError(string.Empty, content);
                        return View("~/Areas/User/Views/Authentication/ResetPassword.cshtml");
                    }
                }
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                // Log exception here if you have logging enabled
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View("~/Areas/User/Views/Authentication/ResetPassword.cshtml"); // Consider using an error view
            }
        }

        //Update Cart If Logout 
        public async Task UpdateCart(List<CartItemRequest> cartItemRequests)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri("https://localhost:7069/api/Cart/UpdateCart"),
                    Content = new StringContent(JsonSerializer.Serialize(cartItemRequests), Encoding.UTF8, "application/json")
                };
                var httpResponse = await client.SendAsync(httpMessage);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine(httpResponse.StatusCode);
                }
            }
            catch (Exception ex)
            {
            }
        }

        //return Role 
        private string GetRoleFromToken(string token)
        {
            try
            {
                // Setting 
                string secretKey = configuration["Jwt:Key"];
                string audience = configuration["Jwt:Audience"];
                string issuer = configuration["Jwt:Issuer"];

                // Thiết lập tham số kiểm tra
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = key
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                // Get Role
                string role = principal.FindFirst(ClaimTypes.Role)?.Value;

                // Return role or a default value if no role is found
                return role ?? string.Empty; // You can return a default value or throw an exception here if needed
            }
            catch (SecurityTokenExpiredException)
            {
                Console.WriteLine("Token đã hết hạn!");
                return string.Empty; // Or some other default value
            }
            catch (SecurityTokenException)
            {
                Console.WriteLine("Token không hợp lệ!");
                return string.Empty; // Or handle it differently
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return string.Empty; // Return an empty string or handle the error as needed
            }
        }

    }

}
