using QLBanHang_API.Services.IService;
using QLBanHang_API.Repositories.IRepository;
using AutoMapper;
using QLBanHang_API.Dto;
using PBL6_QLBH.Models;
namespace QLBanHang_API.Services.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository reviewRepository;
        private readonly IMapper mapper;
        public ReviewService (IReviewRepository reviewRepository,IMapper mapper)
        {
            this.reviewRepository = reviewRepository;
            this.mapper = mapper;
        }

        public async Task<List<ReviewDto>> GetAllReview(Guid? productId)
        {
            if (productId == null)
            {
                return null;
            }
            var reviews = await reviewRepository.GetAllReviewProductAsync(productId);
            var reviewsDto = mapper.Map<List<ReviewDto>>(reviews);
            return reviewsDto;
        }

        public async Task<ReviewDto> AddReview(ReviewDto reviewAdd)
        {
            var review = mapper.Map<Review>(reviewAdd);
            var reviewDomain = await reviewRepository.AddReviewAsync(review);
            var reviewDto = mapper.Map<ReviewDto>(review);
            return reviewDto;
        }

        public async Task<ReviewDto> UpdateReview(ReviewDto reviewUpdate)
        {
            var review = mapper.Map<Review>(reviewUpdate);
            var reviewDomain = await reviewRepository.UpdateReviewAsync(review);
            var reviewDto = mapper.Map<ReviewDto>(reviewDomain);
            return reviewDto;
        }

        public async Task<ReviewDto> DeleteReview(Guid reviewId)
        {
            if (reviewId == null)
            {
                return null;
            }
            var review = await reviewRepository.DeleteReviewAsync(reviewId);
            var reviewDto = mapper.Map<ReviewDto>(review);
            return reviewDto;
        }
    }
}
