using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewsByProduct(Guid productId,int skip ,int take);
    }
}
