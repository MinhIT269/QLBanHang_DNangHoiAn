using Microsoft.AspNetCore.Mvc;
using QLBanHang_API.Service;

namespace QLBanHang_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var product = await _productService.GetAllProductAsync();
            if (product == null)
            {
                return BadRequest();
            }
            return Ok(product);
        }


        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }


        [HttpGet("TotalPagesProduct")]
        public async Task<IActionResult> GetTotalPagesForAllProducts([FromQuery] string? searchQuery) // Tính tổng số trang dựa trên toàn bộ sản phẩm csdl
        {
            var totalPages = await _productService.CountProductAsync(searchQuery ?? string.Empty);
            return Ok(totalPages);
        }


        [HttpGet("GetSearchAndFilteredProducts")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery] string? searchQuery, [FromQuery] int page = 1,
                                             [FromQuery] string sortCriteria = "name",
                                             [FromQuery] bool isDescending = false)
        {
            searchQuery ??= string.Empty;
            var products = await _productService.GetProductsAsync(searchQuery, page, 10, sortCriteria, isDescending);
            return Ok(products);
        }

        [HttpGet("GetProductStats")]
        public async Task<IActionResult> GetProductStats()
        {
            var totalProducts = await _productService.GetTotalProduct();
            var availableProducts = await _productService.AvailableProducts();
            var lowStockProducts = await _productService.GetLowStockProducts();
            var newProducts = await _productService.GetNewProducts();

            return Ok( new
            {
                TotalProducts = totalProducts,
                AvailableProducts = availableProducts,
                LowStockProducts = lowStockProducts,
                NewProducts = newProducts
            });
        }
    }
}