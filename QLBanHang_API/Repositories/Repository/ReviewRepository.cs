using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using QLBanHang_API.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
namespace QLBanHang_API.Repositories.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext dbContext;
        public ReviewRepository(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }
        // Get all Review from one Product
        public async Task<List<Review>> GetAllReviewProductAsync(Guid? productId)
        {
            var reviews = dbContext.Reviews.AsQueryable();
            if (productId != null)
            {
                reviews = reviews.Where(x => x.ProductId == productId);
            }
            else
            {
                return null;
            }
            return await reviews.ToListAsync();
        }
        //Add Review for one product
        public async Task<Review> AddReviewAsync(Review review)
        {
            review.ReviewId = Guid.NewGuid();
            await dbContext.Reviews.AddAsync(review);
            await dbContext.SaveChangesAsync();
            return review;
        }
        //Delete Review 
        public async Task<Review> DeleteReviewAsync(Guid reviewId)
        {
            var review = await dbContext.Reviews.FirstOrDefaultAsync(x => x.ReviewId == reviewId);
            if (review == null)
            {
                return null;
            }
            dbContext.Reviews.Remove(review);
            await dbContext.SaveChangesAsync();
            return review;
        }

        //Update Review 
        public async Task<Review> UpdateReviewAsync(Review reviewUpdate)
        {
            var review = await dbContext.Reviews.FirstOrDefaultAsync(x => x.ReviewId == reviewUpdate.ReviewId);
            if (review == null)
            {
                return null;
            }
            review.Comment = reviewUpdate.Comment;
            review.ReviewDate = reviewUpdate.ReviewDate;
            await dbContext.SaveChangesAsync();
            return review;
        }
    }
}
