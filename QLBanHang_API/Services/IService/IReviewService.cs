using PBL6.Dto;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;

namespace PBL6.Services.IService
{
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetReviewsByProduct(Guid productId,int skip,int size);
        Task<List<ReviewDto>> GetAllReview(Guid? productId);
        Task<ReviewDto> AddReview(AddReviewDto reviewAdd);
        Task<ReviewDto> UpdateReview(UpReviewDto reviewUpdate);
        Task<ReviewDto> DeleteReview(Guid reviewId);
    }
}
