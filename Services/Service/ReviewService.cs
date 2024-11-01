using AutoMapper;
using PBL6.Dto;
using PBL6.Repositories.IRepository;
using PBL6.Services.IService;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using System.Net;

namespace PBL6.Services.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository,IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<List<ReviewDto>> GetReviewsByProduct(Guid productId, int skip, int size)
        {
            var reviews = await _reviewRepository.GetReviewsByProduct(productId, skip, size);
            return _mapper.Map<List<ReviewDto>>(reviews);
        }
    }
}
