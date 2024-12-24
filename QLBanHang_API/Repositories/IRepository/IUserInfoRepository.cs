using PBL6_QLBH.Models;
using QLBanHang_API.Dto;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface IUserInfoRepository
    {
        Task<UserInfo> GetByUserNameAsync(string username);
        Task<UserInfo> UpdateAsync(Guid id, UpdateUserInfoDto userInfo);
        Task<UserInfo> AddUserInfoAsync(UserInfo userInfo);

        Task<UserInfo> GetByUserId(Guid userId);
    }
}

