using Microsoft.AspNetCore.Mvc;
using PBL6.Dto;
using PBL6.Services.Service;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IProductService _productService;

        public ProductController(DataContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        [HttpGet("getProductByName")]
        public async Task<IActionResult> getProductByName([FromQuery] string productName, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            int skip = (page - 1) * size;
            var products = await _productService.GetProductsByName(productName, skip, size);

            return Ok(products);
        }

        [HttpGet("getProductTrending")]
        public async Task<IActionResult> getProductTrending([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            int skip = (page - 1) * size;
            var products = await _productService.GetProductsTrending( skip, size);
            return Ok(products);
        }

        [HttpGet("getProductByCategory")]
        public async Task<IActionResult> getProductsByCatagory([FromQuery] string categoryName, [FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] bool getAll = false)
        {
            int skip = (page - 1) * size;
            var products = await _productService.GetProductsByCategory(categoryName, skip, size,getAll);
            return Ok(products);
        }

        [HttpGet("getProductNotYetReview")]

        public async Task<IActionResult> getProducsNotYetReview([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            int skip = (page - 1) * size;
            var products = await _productService.GetProductNotYetReview(skip, size);
            return Ok(products);
        }

        [HttpGet("getProductNew")]
        public async Task<IActionResult> getProductNew([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            int skip = (page - 1) * size;
            var products = await _productService.GetProductsNew(skip, size);
            return Ok(products);
        }

        [HttpGet("getProductSuggestedByCategory")]
        public async Task<IActionResult> getProductSuggestedByCategory([FromQuery]Guid userId,[FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            Console.WriteLine("Api called");
            int skip = (page - 1) * size;
            var products = await _productService.GetSuggestedProductsByCategory(userId,skip, size);
            Console.WriteLine("Products size:" + products.Count);

            return Ok(products);
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

            return Ok(new
            {
                TotalProducts = totalProducts,
                AvailableProducts = availableProducts,
                LowStockProducts = lowStockProducts,
                NewProducts = newProducts
            });
        }
    }
}
