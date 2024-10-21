using Microsoft.AspNetCore.Mvc;
using QLBanHang_API.Service;

namespace QLBanHang_API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AdminController : ControllerBase
	{
		private const int _itemPerPage = 10;
		private readonly IProductService _productService;
	    public AdminController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet("GetAllProduct")] 
		public async Task<IActionResult> GetAllProduct()
		{
			var product = await _productService.GetAllProductAsync();
			return Ok(product);
		}
	} 
}
