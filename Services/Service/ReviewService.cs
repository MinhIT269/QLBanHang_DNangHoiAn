using AutoMapper;
using PBL6.Dto;
using PBL6.Repositories.IRepository;
using PBL6.Services.IService;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using System.Net;

namespace PBL6.Services.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository reviewRepository;
        private readonly IMapper mapper;

        public ReviewService(IReviewRepository reviewRepository,IMapper mapper)
        {
            this.reviewRepository = reviewRepository;
            this.mapper = mapper;
        }

        public async Task<List<ReviewDto>> GetReviewsByProduct(Guid productId, int skip, int size)
        {
            var reviews = await reviewRepository.GetReviewsByProduct(productId, skip, size);
            return mapper.Map<List<ReviewDto>>(reviews);
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

        public async Task<ReviewDto> AddReview(AddReviewDto reviewAdd)
        {
            var review = mapper.Map<Review>(reviewAdd);
            var reviewDomain = await reviewRepository.AddReviewAsync(review);
            var reviewDto = mapper.Map<ReviewDto>(review);
            return reviewDto;
        }

        public async Task<ReviewDto> UpdateReview(UpReviewDto reviewUpdate)
        {
            var review = mapper.Map<Review>(reviewUpdate);
            var reviewDomain = await reviewRepository.UpdateReviewAsync(review);
            var reviewDto = mapper.Map<ReviewDto>(reviewDomain);
            return reviewDto;
        }

        public async Task<ReviewDto> DeleteReview(Guid reviewId)
        {
            if (reviewId == Guid.Empty)
            {
                return null;
            }
            var review = await reviewRepository.DeleteReviewAsync(reviewId);
            var reviewDto = mapper.Map<ReviewDto>(review);
            return reviewDto;
        }
    }
}
