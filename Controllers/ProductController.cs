using Microsoft.AspNetCore.Mvc;
using PBL6.Dto;
using PBL6.Services.Service;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public async Task<IActionResult> getProductsByCatagory([FromQuery] string categoryName, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            int skip = (page - 1) * size;
            var products = await _productService.GetProductsByCategory(categoryName, skip, size);
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
    }
}
