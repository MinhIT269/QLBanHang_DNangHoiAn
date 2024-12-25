using PBL6.Dto;
using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface IUserInfoRepository
    {
        Task<UserInfo> GetByUserNameAsync(string username);
        Task<UserInfo> UpdateAsync(Guid id, UpdateUserInfoDto userInfo);
        Task<UserInfo> AddUserInfoAsync(UserInfo userInfo);
        Task<UserInfo> GetByUserId(Guid userId);
    }
}
