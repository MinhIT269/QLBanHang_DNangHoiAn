using Microsoft.AspNetCore.Mvc;
using PBL6.Dto;
using PBL6.Services.Service;

namespace PBL6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet("getPromotionCode/{code}")]
        public async Task<ActionResult<PromotionDto>> getPromotionByPromotionCode(String code) 
        {
            var result = await _promotionService.getPromotionByPromotionCode(code);
            return Ok(result);
        }
    }
}
