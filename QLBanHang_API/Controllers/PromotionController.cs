using Microsoft.AspNetCore.Mvc;
using PBL6.Dto;
using PBL6.Services.Service;
using QLBanHang_API.Dto;

namespace PBL6.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class PromotionController : ControllerBase
    {
        private readonly IPromotionService promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            this.promotionService = promotionService;
        }

        [HttpGet("getPromotionCode/{code}")]
        public async Task<ActionResult<PromotionDto>> getPromotionByPromotionCode(String code) 
        {
            var result = await promotionService.getPromotionByPromotionCode(code);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllPromotions()
        {
            var promotionsDTO = await promotionService.GetAllPromotion();
            if (promotionsDTO == null || !promotionsDTO.Any())
            {
                return NotFound();
            }
            return Ok(promotionsDTO);
        }

        // Lấy chương trình khuyến mãi theo mã
        //URL - /api/Promotions/GetOne/code=?
        [HttpGet]
        [Route("GetOne/{code}")]
        public async Task<IActionResult> GetPromotionByCode([FromRoute] Guid code)
        {
            var promotionDTO = await promotionService.GetPromotion(code);
            if (promotionDTO == null)
            {
                return NotFound();
            }
            return Ok(promotionDTO);
        }

        //Update promotion
        //URL -/api/Promotion/Update
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdatePromotion(UpPromotionDto promotionUpdate)
        {
            var promotion = await promotionService.UpdatePromotion(promotionUpdate);
            if (promotion == null)
            {
                return NotFound("Khong the thuc hien Update");
            }
            return Ok(promotion);
        }

        //Add Promotion
        //URL - /api/Promotion/Add
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddPromotion(AddPromotionDto promotionAdd)
        {
            var promotion = await promotionService.AddPromotion(promotionAdd);
            if (promotion == null)
            {
                return BadRequest("Trung Ma Code");
            }
            return Ok(promotion);
        }

        //Delete Promotion
        //URL -/api/Promotion/Delete/id=?
        [HttpDelete]
        [Route("Delete/{id:guid}")]
        public async Task<IActionResult> DeletePromotion(Guid id)
        {
            try
            {
                var promotionDto = await promotionService.DeletePromotion(id);
                return Ok(new { message = "Promotion đã được xóa thành công.", promotion = promotionDto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetFilteredPromotions")]
        public async Task<IActionResult> GetFilteredPromotions([FromQuery] string searchQuery = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 8, [FromQuery] string sortCriteria = "name", [FromQuery] bool isDescending = false)
        {
            var promotions = await promotionService.GetFilteredPromotionsQuery(page, pageSize, searchQuery, sortCriteria, isDescending);
            return Ok(promotions);
        }

        [HttpGet("TotalPagesPromotions")]
        public async Task<IActionResult> GetTotalPagesPromotion([FromQuery] string searchQuery = "")
        {
            var totalRecords = await promotionService.GetTotalPromotionAsync(searchQuery);
            var totalPages = (int)Math.Ceiling((double)totalRecords / 8); // Điều chỉnh số item trên mỗi trang nếu cần
            return Ok(totalPages);
        }

        [HttpGet("GetStats")]
        public async Task<IActionResult> GetPromotionStats()
        {
            try
            {
                var stats = await promotionService.GetPromotionStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
