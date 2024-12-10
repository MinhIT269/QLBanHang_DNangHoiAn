using Microsoft.AspNetCore.Mvc;
using QLBanHang_UI.Helpers;
using QLBanHang_UI.Models;
using System.Text.Json;
using System.Text;

namespace QLBanHang_UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PromotionController : Controller
    {
        private readonly ApiHelper _apiHelper;
        private readonly IHttpClientFactory _httpClientFactory;
        public PromotionController(IHttpClientFactory httpClientFactory)
        {
            _apiHelper = new ApiHelper(httpClientFactory);
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> IndexPromotion()
        {
            var response = await _apiHelper.GetDataFromApi<IEnumerable<PromotionDto>>(
                 "https://localhost:7069/api/Promotion/GetAll",
                 "Không thế tải danh mục"
                 );
            return View(response);
        }
        public IActionResult CreatePromotion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePromotion([FromForm] PromotionDto promotion)
        {
            var client = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7069/api/Promotion/Add"),
                Content = new StringContent(JsonSerializer.Serialize(promotion), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexPromotion");
            }
            // Nếu có lỗi khi tạo, thêm lỗi vào ModelState và trả về trang tạo
            ModelState.AddModelError(string.Empty, "Mã khuyễn mãi đã tồn tại!");
            return View(promotion); // Trả về trang tạo với lỗi
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePromotion(Guid id)
        {
            var promotion = await _apiHelper.GetDataFromApi<PromotionDto>(
                $"https://localhost:7069/api/Promotion/GetOne/{id.ToString()}",
                "Không thể tải mã khuyến mãi");
            return View(promotion);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePromotion([FromForm] PromotionDto promotionDto)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("https://localhost:7069/api/Promotion/Update"),
                Content = new StringContent(JsonSerializer.Serialize(promotionDto), Encoding.UTF8, "application/json")
            };
            var response = await httpClient.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexPromotion");
            }
            ModelState.AddModelError(string.Empty, "Lỗi khi tạo danh mục");
            return View();
        }
    }
}
