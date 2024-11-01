using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface IUserRepository
    {
        public Task<Review> createReview(Review newReview);
    }
}
