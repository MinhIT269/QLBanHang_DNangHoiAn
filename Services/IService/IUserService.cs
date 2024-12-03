using PBL6.Dto;
using PBL6_QLBH.Models;

namespace PBL6.Services.IService
{
    public interface IUserService
    {
        public Task<ReviewDto> createReview(Review review);
        Task<List<UserAdminDto>> GetFilteredUsers(int page, int pageSize, string searchQuery, string sortCriteria, bool isDescending);
        Task<User1Dto> GetUser(string username);
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> UpdateUserInfoByUsernameAsync(string username, UpdateUserDto updatedInfo);
        Task<bool> UpdateUserAsync(string username, string password, string email);
    }
}
