using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanHang_API.Dto;
using QLBanHang_API.Services.IService;
using QLBanHang_API.Services.Service;

namespace QLBanHang_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService promotionService;
        public PromotionController(IPromotionService promotionService)
        {
            this.promotionService = promotionService;
        }

        // Lấy tất cả chương trình khuyến mãi
        // URL - /api/Promotions/GetAll
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
        public async Task<IActionResult> GetPromotionByCode([FromRoute] string code)
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
        public async Task<IActionResult> UpdatePromotion(PromotionDto promotionUpdate)
        {
            var promotion = await promotionService.UpdatePromotion(promotionUpdate);
            if (promotion == null)
            {
                return NotFound();
            }
            return Ok(promotion);
        }

        //Add Promotion
        //URL - /api/Promotion/Add
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddPromotion(PromotionDto promotionAdd)
        {
            var promotion = await promotionService.AddPromotion(promotionAdd);
            if (promotion == null)
            {
                return BadRequest();
            }
            return Ok(promotion);
        }

        //Delete Promotion
        //URL -/api/Promotion/Delete/id=?
        [HttpDelete]
        [Route("Delete/{id:guid}")]
        public async Task<IActionResult> DeletePromotion([FromRoute]Guid id)
        {
            var promotion = await promotionService.DeletePromotion(id);
            if (promotion == null)
            {
                return NotFound();
            }
            return Ok();
        }


    }
}
