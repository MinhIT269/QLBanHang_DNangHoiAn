using PBL6.Dto;
using PBL6_QLBH.Models;

namespace PBL6.Services.IService
{
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetReviewsByProduct(Guid productId,int skip,int size);
    }
}
