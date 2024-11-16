using Microsoft.AspNetCore.Mvc;
using QLBanHang_UI.Helpers;
using QLBanHang_UI.Models;
using System.Text.Json;
using System.Text;

namespace QLBanHang_UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly ApiHelper _apiHelper;
		private readonly IHttpClientFactory _httpClientFactory;
		public CategoryController(IHttpClientFactory httpClientFactory)
		{
			_apiHelper = new ApiHelper(httpClientFactory);
			_httpClientFactory = httpClientFactory;
		}
		public async Task<IActionResult> IndexCategory()
		{
			var response = await _apiHelper.GetDataFromApi<IEnumerable<CategoryDetailDto>>(
				"https://localhost:7069/api/Category/GetAllDetailCategory",
				"Không thế tải danh mục"
				);
			return View(response);
		}

		public IActionResult CreateCategory()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateCategory([FromForm] CategoryDto category)
		{
			var client = _httpClientFactory.CreateClient();
			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://localhost:7069/api/Category/Create"),
				Content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json")
			};
			var response = await client.SendAsync(httpRequestMessage);
			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("IndexCategory");
			}
			ModelState.AddModelError(string.Empty, "Lỗi khi tạo danh mục");
			return RedirectToAction("Error", "Admin");
		}

		[HttpGet("/Admin/Category/UpdateCategory/{id}")]
		public async Task<IActionResult> UpdateCategory(Guid id)
		{
			var category = await _apiHelper.GetDataFromApi<CategoryDto>(
				$"https://localhost:7069/api/Category/{id.ToString()}",
				"Khong the tai danh muc");
			return View(category);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateCategory([FromForm] CategoryDto category)
		{
			var httpClient = _httpClientFactory.CreateClient();
			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Put,
				RequestUri = new Uri("https://localhost:7069/api/Category"),
				Content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json")
			};
			var response = await httpClient.SendAsync(httpRequestMessage);
			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("IndexCategory");
			}
			ModelState.AddModelError(string.Empty, "Lỗi khi tạo danh mục");
			return RedirectToAction("Error", "Admin");
		}
	}
}
