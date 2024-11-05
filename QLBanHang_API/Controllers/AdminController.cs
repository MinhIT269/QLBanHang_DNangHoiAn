using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QLBanHang_API.CustomActionFilters;
using QLBanHang_API.Dto.Request;
using QLBanHang_API.Request;
using QLBanHang_API.Service;
using QLBanHang_API.Services.IService;
using QLBanHang_API.Services.Service;
using System.Text.RegularExpressions;

namespace QLBanHang_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class AdminController : ControllerBase
    {
        private const int _itemPerPage = 10;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IImageService _imageService;
        public AdminController(IProductService productService, ICategoryService categoryService, IBrandService brandService, IImageService imageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _imageService =imageService;
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var product = await _productService.GetAllProductAsync();
            return Ok(product);
        }


        [HttpGet("GetFilteredProducts")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery] string? searchQuery, [FromQuery] int page = 1,
                                                     [FromQuery] string sortCriteria = "name",
                                                     [FromQuery] bool isDescending = false)
        {
            searchQuery ??= string.Empty;
            var products = await _productService.GetProductsAsync(searchQuery, page, 10, sortCriteria, isDescending);
            return Ok(products);
        }

        [HttpGet("TotalPagesProduct")]
        public async Task<IActionResult> GetTotalPagesForAllProducts([FromQuery] string? searchQuery) // Tính tổng số trang dựa trên toàn bộ sản phẩm csdl
        {
            var totalPages = await _productService.CountProductAsync(searchQuery ?? string.Empty);
            return Ok(totalPages);
        }

        [HttpPost("CreateProduct")]
        [ValidateModel]
        public async Task<IActionResult> CreateProduct([FromForm] ProductRequest model, IFormFile ImageUrl, IList<IFormFile> additionalImages)
        {
            try
            {
                model.Description = await _imageService.ProcessDescriptionAndUploadImages(model.Description!, model.ProductId);
                bool isSuccess = await _productService.AddProductAsync(model, ImageUrl, additionalImages);
                if (isSuccess)
                {
                    return Ok("Product created successfully.");
                }
                else
                {
                    return BadRequest("Failed to create product. Please check the provided data.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("UpdateProduct")]
        [ValidateModel]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductRequest model, IFormFile? mainImage, IList<IFormFile> additionalImages, [FromForm] string? oldImageUrlsJson)
        {
            try
            {
                var oldImageUrls = string.IsNullOrEmpty(oldImageUrlsJson) ? new List<string>() 
                    : Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(oldImageUrlsJson);

                model.Description = await _imageService.ProcessDescriptionAndUploadImages(model.Description!, model.ProductId);
                bool isSuccess = await _productService.UpdateProductAsync(model, mainImage, additionalImages, oldImageUrls);
                if (isSuccess)
                {
                    return Ok("Product updated successfully.");
                }
                else
                {
                    return BadRequest("Failed to update product. Please check the provided data.");
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Trả về 404 nếu không tìm thấy sản phẩm
            }
        }


        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
            {
                return NotFound(); // Trả về 404 nếu ko tìm thấy sản phẩm 
            }
            return Ok();
        }

        [HttpPost("Upload-Picture")]
        public async Task<IActionResult> UploadImages(IFormFile files, Guid productId)
        {
            if (files == null)
            {
                return BadRequest("No files were uploaded.");
            }

            var uploadedImageUrls = await _imageService.UploadImageAsync(files, productId);
            return Ok(new { urls = uploadedImageUrls }); // Trả về danh sách đường dẫn hình ảnh dưới dạng JSON
        }
    }
}
