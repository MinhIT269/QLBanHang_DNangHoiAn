using Microsoft.AspNetCore.Mvc;
using PBL6.Dto.Request;
using PBL6.Dto;
using PBL6.Services.IService;

namespace PBL6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService brandService;
        private readonly ILocationService locationService;
        public BrandController(IBrandService brandService, ILocationService locationService)
        {
            this.brandService = brandService;
            this.locationService = locationService;
        }

        // api/Brands/GetAllBrands
        [HttpGet("GetAllBrands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var brandDTO = await brandService.GetAllBrands();
            if (brandDTO == null)
            {
                return NotFound();
            }
            return Ok(brandDTO);
        }

        // Get Brand - /api/Brands/Get/brandName=?
        [HttpGet]
        [Route("Get/{brandName}")]
        public async Task<IActionResult> GetBrandByName([FromRoute] string brandName)
        {
            var brandDTO = await brandService.GetBrandByName(brandName);
            if (brandDTO == null)
            {
                return NotFound();
            }
            return Ok(brandDTO);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(Guid id)
        {
            var brand = await brandService.GetBrandByIdAsync(id);
            return Ok(brand);
        }

        [HttpGet("GetFilteredBrands")]
        public async Task<IActionResult> GetFilteredBrands([FromQuery] string searchQuery = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 8, [FromQuery] string sortCriteria = "name", [FromQuery] bool isDescending = false)
        {
            var (brands, totalRecords) = await brandService.GetFilteredCategoriesAsync(page, pageSize, searchQuery, sortCriteria, isDescending);
            return Ok(brands);
        }

        [HttpGet("TotalPagesBrands")]
        public async Task<IActionResult> GetTotalPagesCategory([FromQuery] string searchQuery = "")
        {
            var totalRecords = await brandService.GetTotalBrandsAsync(searchQuery);
            var totalPages = (int)Math.Ceiling((double)totalRecords / 8); // Điều chỉnh số item trên mỗi trang nếu cần
            return Ok(totalPages);
        }

        [HttpDelete]
        [Route("Delete/{id:guid}")]
        public async Task<IActionResult> DeleteBrandById([FromRoute] Guid id)
        {
            // Kiểm tra xem thương hiệu có tồn tại không
            var brand = await brandService.GetBrandByIdAsync(id);
            if (brand == null)
            {
                return NotFound("Thương hiệu không tồn tại.");
            }

            // Kiểm tra xem thương hiệu có đang được sử dụng không (ví dụ, có sản phẩm nào liên quan đến thương hiệu này không)
            var productsUsingBrand = await brandService.HasProductsByBrandIdAsync(id);
            if (productsUsingBrand)
            {
                return BadRequest("Thương hiệu này đang được sử dụng bởi các sản phẩm, không thể xóa!");
            }

            // Thực hiện xóa thương hiệu
            var deletedBrand = await brandService.DeleteBrand(id);
            if (deletedBrand == null)
            {
                return BadRequest("Không thể xóa thương hiệu.");
            }

            return Ok("Thương hiệu đã được xóa thành công!");
        }

        // Update Brand - /api/Brands/Update/id=?
        [HttpPut]
        [Route("Update/{id:guid}")]
        public async Task<IActionResult> UpdateBrandById([FromRoute] Guid id, [FromBody] UpBrandDto brand)
        {
            var brandDTO = await brandService.UpdateBrand(id, brand);
            if (brandDTO == null)
            {
                return NotFound();
            }
            return Ok(brandDTO);
        }


        // Create Brand - /api/Brands/Add
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddBrand([FromBody] BrandRequest brand)
        {
            if (await brandService.IsBrandNameExists(brand.BrandName!))
            {
                return BadRequest("Tên thương hiệu đã tồn tại.");
            }
            var brandDTO = await brandService.AddBrand(brand);
            if (brandDTO == null)
            {
                return NotFound();
            }
            return Ok(brandDTO);
        }
    }
}
