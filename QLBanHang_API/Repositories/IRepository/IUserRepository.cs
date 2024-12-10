using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUserAsync();
        Task<User> DeleteUserAsync(string userName);
        Task<User> AddUserAsync(User user);
        IQueryable<User> GetFilteredUsers(string searchQuery, string sortCriteria, bool isDescending);
    }
}
