using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Service;

namespace QLBanHang_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService brandService;
        public BrandController(IBrandService brandService)
        {
            this.brandService = brandService;
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

        // Delete Brand - /api/Brands/Delete/id=?
        [HttpDelete]
        [Route("Delete/{id:guid}")]
        public async Task<IActionResult> DeleteBrandById([FromRoute] Guid id)
        {
            var brandDTO = await brandService.DeleteBrand(id);
            if (brandDTO == null)
            {
                return NotFound();
            }
            return Ok();
        }

        // Update Brand - /api/Brands/Update/id=?
        [HttpPut]
        [Route("Update")]
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
        public async Task<IActionResult> AddBrand([FromBody] AddBrandDto brand)
        {
            var brandDTO = await brandService.AddBrand(brand);
            if (brandDTO == null)
            {
                return NotFound();
            }
            return Ok(brandDTO);
        }
    }
}
