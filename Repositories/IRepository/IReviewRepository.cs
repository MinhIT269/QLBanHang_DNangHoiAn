using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewsByProduct(Guid productId,int skip ,int take);
        Task<List<Review>> GetAllReviewProductAsync(Guid? productId);
        Task<Review> AddReviewAsync(Review review);
        Task<Review> DeleteReviewAsync(Guid reviewId);
        Task<Review> UpdateReviewAsync(Review review);
    }
}
