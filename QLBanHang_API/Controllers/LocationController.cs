using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanHang_API.Dto;
using QLBanHang_API.Services.IService;

namespace QLBanHang_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService locationService;
        public LocationController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        // Lay Danh Sach Location = Name
        // URL - /api/Location/GetList/name=?
        [HttpGet]
        [Route("GetList/{name}")]
        public async Task<IActionResult> GetListLocationByName([FromRoute] string? name)
        {
            var locationsDTO = await locationService.GetListByName(name);
            if (locationsDTO == null || !locationsDTO.Any())
            {
                return NotFound();
            }
            return Ok(locationsDTO);
        }

        // Lay 1 Location cu the
        // URL - /api/Location/GetLocation/id=?
        [HttpGet]
        [Route("GetLocation/{id:Guid}")]
        public async Task<IActionResult> GetLocationById([FromRoute] Guid id)
        {
            var locationDTO = await locationService.GetLocationById(id);
            if (locationDTO == null)
            {
                return NotFound();
            }
            return Ok(locationDTO);
        }

        // Update Location
        // URL - /api/Location/Update?id = ?
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateLocation([FromQuery] Guid id, [FromBody] UpLocationDto locationUpdate)
        {
            var locationDTO = await locationService.UpdateLocation(id, locationUpdate);
            if (locationDTO == null)
            {
                return NotFound();
            }
            return Ok(locationDTO);
        }

        //Delete Location
        //URL -/api/Location/Delete/id=?
        [HttpDelete]
        [Route("Delete/{id:guid}")]
        public async Task<IActionResult> DeleteLocation([FromRoute]Guid id)
        {
            var location = await locationService.DeleteLocation(id);
            if (location == null)
            {
                return NotFound();
            }
            return Ok();
        }

        //Add Location
        //URL -/api/Location/Add
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLocation([FromBody] AddLocationDto addLocation)
        {
            var location = await locationService.AddLocation(addLocation);
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

    }
}
