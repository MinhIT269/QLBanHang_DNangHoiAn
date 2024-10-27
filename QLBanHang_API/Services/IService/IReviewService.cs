using System.Runtime.CompilerServices;
using QLBanHang_API.Dto;
namespace QLBanHang_API.Services.IService
{
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetAllReview(Guid? productId);
        Task<ReviewDto> AddReview(AddReviewDto reviewAdd);
        Task<ReviewDto> UpdateReview(UpReviewDto reviewUpdate);
        Task<ReviewDto> DeleteReview (Guid reviewId);
    }
}
