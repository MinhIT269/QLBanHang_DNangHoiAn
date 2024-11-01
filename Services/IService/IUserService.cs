using PBL6.Dto;
using PBL6_QLBH.Models;

namespace PBL6.Services.IService
{
    public interface IUserService
    {
        public Task<ReviewDto> createReview(Review review);
    }
}
