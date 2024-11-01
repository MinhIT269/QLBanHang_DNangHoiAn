using Microsoft.EntityFrameworkCore;
using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Repositories.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context= context;
        }


        public async Task<List<Review>> GetReviewsByProduct(Guid productId, int skip, int take)
        {
            return await _context.Reviews.Where( r => r.ProductId == productId)
                .Include(r => r.User)
                .Include(r => r.Product)
                .OrderByDescending(r => r.ReviewDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}
