using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetAllReviewProductAsync(Guid? productId);
        Task<Review> AddReviewAsync(Review review);
        Task<Review> DeleteReviewAsync(Guid reviewId);
        Task<Review> UpdateReviewAsync(Review review);
    }
}
