using Microsoft.AspNetCore.Mvc;
using QLBanHang_UI.Helpers;
using QLBanHang_UI.Models;
using QLBanHang_UI.Models.Request;
using System.Net.Http.Headers;
using System.Text.Json;

namespace QLBanHang_UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly ApiHelper _apiHelper;
		private readonly IHttpClientFactory _httpClientFactory;
		public ProductController(IHttpClientFactory httpClientFactory)
		{
			_apiHelper = new ApiHelper(httpClientFactory);
			_httpClientFactory = httpClientFactory;
		}
		public async Task<IActionResult> IndexProduct()
		{
			var response = await _apiHelper.GetDataFromApi<IEnumerable<ProductDto>>(
				"https://localhost:7069/api/admin/GetFilteredProducts?searchQuery=&page=1&sortCriteria=name&isDescending=false",
				"Không thể tải danh sách sản phẩm."
				);
			return View(response);
		}

		// Trang them sp
		[HttpGet]
		public async Task<IActionResult> CreateProduct()
		{
			var categories = await _apiHelper.GetDataFromApi<IEnumerable<CategoryDto>>(
				"https://localhost:7069/api/Category/GetAllCategories",
				"Không thể tải danh sách danh mục."
				);

			var brands = await _apiHelper.GetDataFromApi<IEnumerable<BrandDto>>(
				"https://localhost:7069/api/Brand/GetAllBrands",
				"Không thể tải danh sách thương hiệu."
				);
			ViewBag.Categories = categories ?? new List<CategoryDto>();
			ViewBag.Brands = brands ?? new List<BrandDto>();
			return View();
		}

		// Them san pham admin
		[HttpPost]
		public async Task<IActionResult> CreateProduct([FromForm] ProductRequest model, IFormFile ImageUrl, IList<IFormFile> additionalImages)
		{
			model.CreatedDate = DateTime.Now.Date;
			model.ProductId = Guid.NewGuid();

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // Trả về lỗi nếu model vẫn không hợp lệ
			}
			var content = new MultipartFormDataContent(); // Tạo MultipartFormDataContent để gửi tới API

			// Thêm các trường vào content
			content.Add(new StringContent(model.ProductId.ToString()), "ProductId");
			content.Add(new StringContent(model.Name!), "Name");
			content.Add(new StringContent(model.MetaTitle!), "MetaTitle");
			content.Add(new StringContent(model.Description!), "Description");
			content.Add(new StringContent(model.MetaDescription!), "MetaDescription");
			content.Add(new StringContent(model.CreatedDate.ToString()), "CreatedDate");
			content.Add(new StringContent(model.Price.ToString()), "Price");
			content.Add(new StringContent(model.PromotionPrice?.ToString()!), "PromotionPrice");
			content.Add(new StringContent(model.Stock.ToString()), "Stock");
			content.Add(new StringContent(model.BrandId.ToString()), "BrandId");
			content.Add(new StringContent(model.Warranty!), "Warranty");

			foreach (var category in model.CategoryIds!)
			{
				content.Add(new StringContent(category.ToString()), "CategoryIds");
			}
			// Thêm hình ảnh chính
			if (ImageUrl != null)
			{
				var mainImageContent = new StreamContent(ImageUrl.OpenReadStream())
				{
					Headers = { ContentType = new MediaTypeHeaderValue(ImageUrl.ContentType) }
				};
				content.Add(mainImageContent, "ImageUrl", ImageUrl.FileName);
			}

			// Thêm các hình ảnh phụ
			if (additionalImages != null)
			{
				foreach (var additionalImage in additionalImages)
				{
					if (additionalImage.Length > 0) // Kiểm tra nếu hình ảnh không rỗng
					{
						var additionalImageContent = new StreamContent(additionalImage.OpenReadStream())
						{
							Headers = { ContentType = new MediaTypeHeaderValue(additionalImage.ContentType) }
						};
						content.Add(additionalImageContent, "additionalImages", additionalImage.FileName);
					}
				}
			}

			var httpClient = _httpClientFactory.CreateClient(); // Tạo HttpClient từ IHttpClientFactory
			var apiUrl = "https://localhost:7069/api/Admin/CreateProduct";

			// Gửi request tới API
			var response = await httpClient.PostAsync(apiUrl, content);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("IndexProduct", "Product"); // Chuyển hướng đến trang sản phẩm
			}

			ModelState.AddModelError("", "Failed to create product. Please try again."); // Thêm lỗi vào ModelState
			return RedirectToAction("Error", "Admin"); // Trả lại view với model để hiển thị lỗi nếu có
		}


		[HttpGet]
		public async Task<IActionResult> EditProduct(Guid id)
		{
			var product = await _apiHelper.GetDataFromApi<ProductDto>(
				$"https://localhost:7069/api/Product/GetProductById/{id.ToString()}",
				"Khong the tai san pham");
			var categories = await _apiHelper.GetDataFromApi<IEnumerable<CategoryDto>>(
				"https://localhost:7069/api/Category/GetAllCategories",
				"Không thể tải danh sách danh mục."
				);

			var brands = await _apiHelper.GetDataFromApi<IEnumerable<BrandDto>>(
				"https://localhost:7069/api/Brand/GetAllBrands",
				"Không thể tải danh sách thương hiệu."
				);
			ViewBag.Categories = categories ?? new List<CategoryDto>();
			ViewBag.Brands = brands ?? new List<BrandDto>();
			if (product is not null)
			{
				return View(product);
			}
			return RedirectToAction("IndexProduct", "Product");
		}


		[HttpPost]
		public async Task<IActionResult> EditProduct([FromForm] ProductRequest model, IFormFile mainImage, IList<IFormFile> additionalImages, List<string> oldImageUrls)
		{
			var httpClient = _httpClientFactory.CreateClient();
			var apiUrl = "https://localhost:7069/api/Admin/UpdateProduct";
			var content = new MultipartFormDataContent();

			// Thêm thông tin sản phẩm vào content
			content.Add(new StringContent(model.ProductId.ToString()), "ProductId");
			content.Add(new StringContent(model.Name ?? ""), "Name");
			content.Add(new StringContent(model.MetaTitle ?? ""), "MetaTitle");
			content.Add(new StringContent(model.Description ?? ""), "Description");
			content.Add(new StringContent(model.MetaDescription ?? ""), "MetaDescription");
			content.Add(new StringContent(model.Price.ToString()), "Price");
			content.Add(new StringContent(model.PromotionPrice?.ToString() ?? "0"), "PromotionPrice");
			content.Add(new StringContent(model.Stock.ToString()), "Stock");
			content.Add(new StringContent(model.BrandId.ToString()), "BrandId");
			content.Add(new StringContent(model.CreatedDate.ToString("o")), "CreatedDate"); // ISO 8601
			content.Add(new StringContent(model.Warranty ?? ""), "Warranty");

			foreach (var categoryId in model.CategoryIds ?? new List<Guid>())
			{
				content.Add(new StringContent(categoryId.ToString()), "CategoryIds");
			}

			// Xử lý ảnh chính
			if (mainImage != null)
			{
				oldImageUrls?.RemoveAt(0);  // Xóa ảnh cũ đầu tiên nếu có mainImage mới
				var mainImageContent = new StreamContent(mainImage.OpenReadStream())
				{
					Headers = { ContentType = new MediaTypeHeaderValue(mainImage.ContentType) }
				};
				content.Add(mainImageContent, "mainImage", mainImage.FileName);
			}

			// Xử lý ảnh phụ
			if (additionalImages != null)
			{
				foreach (var image in additionalImages)
				{
					if (image.Length > 0)
					{
						var imageContent = new StreamContent(image.OpenReadStream())
						{
							Headers = { ContentType = new MediaTypeHeaderValue(image.ContentType) }
						};
						content.Add(imageContent, "additionalImages", image.FileName);
					}
				}
			}

			// Chuyển oldImageUrls thành chuỗi JSON và thêm vào content
			if (oldImageUrls?.Count > 0)
			{
				var oldImageUrlsJson = JsonSerializer.Serialize(oldImageUrls);
				content.Add(new StringContent(oldImageUrlsJson), "oldImageUrlsJson");
			}

			// Gửi request
			var response = await httpClient.PutAsync(apiUrl, content);
			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("IndexProduct", "Product");
			}
			else
			{
				var errorMessage = await response.Content.ReadAsStringAsync();
				Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
				return RedirectToAction("Error", "Admin");
			}
		}
	}
}
