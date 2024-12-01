using QLBanHang_API.Dto;

namespace QLBanHang_API.Services.IService
{
    public interface IUserService
    {
        Task<List<UserAdminDto>> GetFilteredUsers(int page, int pageSize, string searchQuery, string sortCriteria, bool isDescending);
    }
}
