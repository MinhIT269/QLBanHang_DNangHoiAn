using PBL6_QLBH.Models;
using QLBanHang_API.Dto;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface IUserInfoRepository
    {
        Task<UserInfo> GetByUserNameAsync(string username);
        Task<UserInfo> UpdateAsync(string username, UpdateUserInfoDto userInfo);
        Task<UserInfo> AddUserInfoAsync(string username,UserInfo userInfo);
    }
}

