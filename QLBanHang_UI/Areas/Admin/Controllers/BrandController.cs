using Microsoft.AspNetCore.Mvc;
using QLBanHang_UI.Models.Request;
using System.Text.Json;
using System.Text;
using QLBanHang_UI.Helpers;

namespace QLBanHang_UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
		private readonly ApiHelper _apiHelper;
		private readonly IHttpClientFactory _httpClientFactory;
        public BrandController(IHttpClientFactory httpClientFactory) 
        {
            _apiHelper = new ApiHelper(httpClientFactory);
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult IndexBrand()
        {
            return View();
        }

        public IActionResult CreateBrand()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromForm] BrandRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7069/api/Brand/Add"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexBrand");
            }
            return RedirectToAction("Error", "Admin");
        }
        public async Task<IActionResult> UpdateBrand(Guid id)
        {
			var brand = await _apiHelper.GetDataFromApi<BrandRequest>(
				$"https://localhost:7069/api/Brand/{id.ToString()}",
				"Khong the tai danh muc");
			return View(brand);
        }
    }
}
