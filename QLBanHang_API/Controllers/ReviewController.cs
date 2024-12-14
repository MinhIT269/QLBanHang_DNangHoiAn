using Microsoft.AspNetCore.Mvc;
using PBL6.Dto;
using PBL6.Services.IService;
using PBL6_QLBH.Models;

namespace PBL6.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<List<ReviewDto>>> getReviewByProduct(Guid productId, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            int skip = (page - 1) * size;
            var reviews = await _reviewService.GetReviewsByProduct(productId, skip, size);
            Console.WriteLine("Count:" +reviews.Count);
            return Ok(reviews);
        }

    }
}
