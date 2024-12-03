using PBL6.Dto;
using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface IUserInfoRepository
    {
        Task<UserInfo> GetByUserNameAsync(string username);
        Task<UserInfo> UpdateAsync(string username, UpdateUserInfoDto userInfo);
        Task<UserInfo> AddUserInfoAsync(string username, UserInfo userInfo);
    }
}
