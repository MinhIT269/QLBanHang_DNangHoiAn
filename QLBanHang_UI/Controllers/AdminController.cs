using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanHang_UI.Models;
using System.Net.Cache;

namespace QLBanHang_UI.Controllers
{
	public class AdminController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public AdminController(IHttpClientFactory httpClientFactory)
        {
			_httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
		{
			return View();
		}
		public async Task<IActionResult> Product()
		{
			List<ProductDto> response = new List<ProductDto>().ToList();
			try
			{
				var client = _httpClientFactory.CreateClient();
				var httpResponse = await client.GetAsync("https://localhost:7069/api/admin/GetFilteredProducts?searchQuery=&page=1&sortCriteria=name&isDescending=false");
				// Kiểm tra nếu response thành công
				if (httpResponse.IsSuccessStatusCode)
				{
					// Đọc nội dung và kiểm tra null
					var products = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
					if (products != null)
					{
						response.AddRange(products);
					}
				}
				else
				{
					// Nếu không thành công, có thể log lỗi hoặc xử lý khác
					ModelState.AddModelError(string.Empty, "Không thể tải danh sách sản phẩm.");
				}
			}
			catch (Exception ex)
			{

				throw;
			}
			return View(response);
		}
	}
}
