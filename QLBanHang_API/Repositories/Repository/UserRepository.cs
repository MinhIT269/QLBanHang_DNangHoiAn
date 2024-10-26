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
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == userName);
            if (user == null)
            {
                return null;
            }
            dbContext.Users.Remove(user);
            return user;
        }

        public async Task<User> AddUserAsync(User user)
        {

            user.UserId = Guid.NewGuid();
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }
    }
}
