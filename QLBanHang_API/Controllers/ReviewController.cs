using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Services.IService;

namespace QLBanHang_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService reviewService;
        public ReviewController (IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        // Get all reviews for a product
        // URL - /api/Review/GetAll/id=?
        [HttpGet]
        [Route("GetAll/{id:guid}")]
        public async Task<IActionResult> GetAllReviewProduct([FromRoute] Guid id)
        {
            var reviewsDTO = await reviewService.GetAllReview(id);
            if (reviewsDTO == null || !reviewsDTO.Any())
            {
                return NotFound();
            }
            return Ok(reviewsDTO);
        }

        // Add a review for a product
        // URL - /api/Review/Add
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddReviewForProduct([FromBody] AddReviewDto addReviewDTO)
        {
            var reviewDTO = await reviewService.AddReview(addReviewDTO);
            if (reviewDTO == null)
            {
                return NotFound();
            }
            return Ok(reviewDTO);
        }

        // Delete a review
         //URL - /api/Review/Delete/id=?
        [HttpDelete]
        [Route("Delete/{id:guid}")]
        public async Task<IActionResult> DeleteReview([FromRoute] Guid id)
        {
            var review = await reviewService.DeleteReview(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok();
        }

        // Update a review
        // URL - /api/Review/Update
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateReview([FromBody] UpReviewDto reviewUpdate)
        {
            var reviewDTO = await reviewService.UpdateReview(reviewUpdate);
            if (reviewDTO == null)
            {
                return NotFound();
            }
            return Ok(reviewDTO);
        }
    }
}
