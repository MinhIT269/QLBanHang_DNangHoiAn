using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Dto.Request;
using QLBanHang_API.Service;
using QLBanHang_API.Services.IService;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace QLBanHang_API.Controllers
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

        // api/Brand/GetAllBrands
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

        //Update Brand - /api/Brands/Update/id=?
        [HttpPut]
        [Route("UpdateBrandLocation")]
        public async Task<IActionResult> UpdateBrandLocationById([FromBody] JsonArray array)
        {
            var upBrand = new UpBrandDto();
            var upLocations = new List<UpLocationDto>();
            foreach (JsonNode item in array)
            {
                var type = item["Type"]?.ToString();
                if (type == "Brand")
                {
                    upBrand = item.Deserialize<UpBrandDto>();
                }
                else if(type == "Location")
                {
                    var updateLocation = item.Deserialize<UpLocationDto>();
                    upLocations.Add(updateLocation);
                }
                else
                {
                    Console.WriteLine("Unrecognized type: " + type);
                }
            }
            var locations = await locationService.UpdateListLocation(upLocations);
            var brandDto = await brandService.UpdateBrand(upBrand);
            
            if (brandDto != null && locations.Any())
            {
                return Ok(brandDto);
            }
            else if (brandDto != null)
            {
                return Ok(brandDto);
            }
            else if (upLocations.Any())
            {
                return Ok(locations);
            }

            return BadRequest();
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
