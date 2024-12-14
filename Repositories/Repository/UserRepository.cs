    using Microsoft.EntityFrameworkCore;
using PBL6.Dto;
using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Repositories.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly DataContext dbContext;
        public UserRepository(DataContext context)
        {
            dbContext = context;
        }

        public async Task<Review> createReview(Review newReview)
        {
            newReview.ReviewId = new Guid();
            newReview.UserId =Guid.Parse("d4e56743-ff2c-41d3-957d-576e9f574c5d") ;
            newReview.ReviewDate = DateTime.Now;    

            var result = await dbContext.Reviews.AddAsync(newReview); 
            await dbContext.SaveChangesAsync();

            return await dbContext.Reviews
            .Include(r => r.Product) 
            .Include(r => r.User)    
            .FirstOrDefaultAsync(r => r.ReviewId == result.Entity.ReviewId);
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            var users = await dbContext.Users.Include("Role").ToListAsync();
            return users;

        }
        public async Task<User> DeleteUserAsync(string userName)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                return null;
            }
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> AddUserAsync(User user)
        {

            user.UserId = Guid.NewGuid();
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public IQueryable<User> GetFilteredUsers(string searchQuery, string sortCriteria, bool isDescending)
        {
            var query = dbContext.Users
                .Include(c => c.UserInfo)
                .Include(c => c.Orders).AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => EF.Functions.Collate(c.UserName, "SQL_Latin1_General_CP1_CI_AI")!.Contains(searchQuery) || c.PhoneNumber!.Contains(searchQuery));
            }

            query = sortCriteria switch
            {
                "name" => isDescending ? query.OrderByDescending(c => c.UserName) : query.OrderBy(c => c.UserName),
                "phone" => isDescending ? query.OrderByDescending(c => c.UserInfo!.PhoneNumber) : query.OrderBy(c => c.UserInfo!.PhoneNumber),
                _ => query
            };
            return query;
        }

        public async Task<User1Dto> GetUserByUsername(string username)
        {
            var user = await dbContext.Users
                .Where(u => u.UserName == username)
                .Select(u => new User1Dto
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.UserInfo.FirstName,
                    LastName = u.UserInfo.LastName,
                    Address = u.UserInfo.Address,
                    PhoneNumber = u.UserInfo.PhoneNumber,
                    Gender = u.UserInfo.Gender,
                    PasswordHash = u.PasswordHash
                })
                .FirstOrDefaultAsync();

            return user;
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<UserInfo> GetUserInfoByUserIdAsync(Guid userId)
        {
            return await dbContext.UserInfos.FirstOrDefaultAsync(ui => ui.UserId == userId);
        }

        public async Task UpdateUserInfoAsync(UserInfo userInfo)
        {
            dbContext.UserInfos.Update(userInfo);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }
    }
}
