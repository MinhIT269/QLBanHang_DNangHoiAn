    using Microsoft.EntityFrameworkCore;
using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Repositories.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Review> createReview(Review newReview)
        {
            newReview.ReviewId = new Guid();
            newReview.UserId =Guid.Parse("d4e56743-ff2c-41d3-957d-576e9f574c5d") ;
            newReview.ReviewDate = DateTime.Now;    

            var result = await _context.Reviews.AddAsync(newReview); 
            await _context.SaveChangesAsync();

            return await _context.Reviews
            .Include(r => r.Product) 
            .Include(r => r.User)    
            .FirstOrDefaultAsync(r => r.ReviewId == result.Entity.ReviewId);
        }
    }
}
