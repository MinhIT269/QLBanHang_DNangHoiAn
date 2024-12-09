using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using Microsoft.EntityFrameworkCore;
using QLBanHang_API.Repositories.IRepository;
namespace QLBanHang_API.Repositories.Repository
{
    public class UserRepository :IUserRepository
    {
        private readonly DataContext dbContext;
        public UserRepository(DataContext dbContext)
        {
            this.dbContext = dbContext;
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
            if(!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => EF.Functions.Collate(c.UserName, "SQL_Latin1_General_CP1_CI_AI")!.Contains(searchQuery) || c.UserInfo!.PhoneNumber!.Contains(searchQuery) || EF.Functions.Collate(c.Email, "SQL_Latin1_General_CP1_CI_AI")!.Contains(searchQuery) || c.UserId.ToString().Substring(0, 8).Contains(searchQuery));
            }

            query = sortCriteria switch
            {
                "name" => isDescending ? query.OrderByDescending(c => c.UserName) : query.OrderBy(c => c.UserName),
                "phone" => isDescending ? query.OrderByDescending(c => c.UserInfo!.PhoneNumber) : query.OrderBy(c => c.UserInfo!.PhoneNumber),   
                "email" => isDescending ? query.OrderByDescending(c => c.Email) : query.OrderBy(c => c.Email),
                _ => query
            };
            return query;
        } 
    }
}
