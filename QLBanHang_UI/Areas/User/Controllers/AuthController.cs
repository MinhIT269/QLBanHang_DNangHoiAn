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
namespace QLBanHang_UI.Areas.User.Controllers
{
	[Area("User")]
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        public AuthController(IHttpClientFactory httpClientFactory)
        {
                this.httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Areas/User/Views/Login.cshtml");
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
                    return View("~/Areas/User/Views/Login.cshtml");
                }

                var response = await httpResponse.Content.ReadFromJsonAsync<LoginResponse>();
                Console.WriteLine(response.JwtToken);
                if (response == null)
                {
                    ModelState.AddModelError("", "Login failed:");
                    return View("~/Areas/User/Views/Login.cshtml");
                }
                Response.Cookies.Append("auth_token", response.JwtToken, new CookieOptions { HttpOnly = true, Expires = DateTime.Now.AddHours(2)});
                Response.Cookies.Append("UserId", response.UserId.ToString(), new CookieOptions { HttpOnly = true, Expires = DateTime.Now.AddHours(1) });
                return RedirectToAction("Dashboard", "Home");
            }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                    ModelState.AddModelError("", "Có lỗi xảy ra khi đăng nhập: " + ex.Message);
                    return View("~/Areas/User/Views/Login.cshtml");
                }

        }

        //// Logout 
        //[HttpGet]
        //public IActionResult Logout()
        //{

        //}
        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Areas/User/Views/Register.cshtml");  // Chắc chắn rằng đường dẫn view chính xác
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                // Nếu ModelState không hợp lệ, trả về view cùng dữ liệu người dùng đã nhập
                return View("~/Areas/User/Views/Register.cshtml", registerRequest);
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
                    return View("~/Areas/User/Views/Register.cshtml");
                }

                // Nếu đăng ký thành công, chuyển hướng tới trang đăng nhập
                return RedirectToAction("Login", "Auth", new { area = "User" });
            }
            catch (Exception ex)
            {
                // Thêm lỗi chung vào ModelState khi có ngoại lệ
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again later.");

                // Trả về view với lỗi ngoại lệ
                return View("~/Areas/User/Views/Register.cshtml", registerRequest);
            }
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View("~/Areas/User/Views/ResetPassword.cshtml");
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
                    return View("~/Areas/User/Views/ResetPassword.cshtml");
                }

                var content = await response.Content.ReadAsStringAsync();
                ViewBag.Message = content;
                return View("~/Areas/User/Views/ResetPassword.cshtml"); // You may use a specific view or pass content as a model if needed
            }
            catch (Exception ex)
            {
                // Log exception here if you have logging enabled
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View("~/Areas/User/Views/ResetPassword.cshtml"); // Consider using an error view
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
                        return View("~/Areas/User/Views/ResetPassword.cshtml");
                    }
                }
                return RedirectToAction("Login","Auth");
            }
            catch(Exception ex)
            {
                // Log exception here if you have logging enabled
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View("~/Areas/User/Views/ResetPassword.cshtml"); // Consider using an error view
            }
        }

    }

}
