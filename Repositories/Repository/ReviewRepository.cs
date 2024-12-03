using Microsoft.EntityFrameworkCore;
using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Repositories.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext dbContext;

        public ReviewRepository(DataContext context)
        {
            dbContext= context;
        }


        public async Task<List<Review>> GetReviewsByProduct(Guid productId, int skip, int take)
        {
            return await dbContext.Reviews.Where( r => r.ProductId == productId)
                .Include(r => r.User)
                .Include(r => r.Product)
                .OrderByDescending(r => r.ReviewDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
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
