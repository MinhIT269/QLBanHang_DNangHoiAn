using Microsoft.AspNetCore.Mvc;
using QLBanHang_UI.Helpers;
using QLBanHang_UI.Models;

namespace QLBanHang_UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly ApiHelper _apiHelper;
        private readonly IHttpClientFactory _httpClientFactory;
        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _apiHelper = new ApiHelper(httpClientFactory);
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult IndexCustomer()
        {
            return View();
        }

        public async Task<IActionResult> DetailCustomer([FromQuery] string userName)
        {
            var user = await _apiHelper.GetDataFromApi<UserInfoDto>(
                $"https://localhost:7069/api/UserInfo/Get/{userName}",
                "Khong the tai khach hang");
            return View(user);
        }
    }
}
