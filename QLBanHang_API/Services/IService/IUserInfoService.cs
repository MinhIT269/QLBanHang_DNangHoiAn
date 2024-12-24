using PBL6_QLBH.Models;
using QLBanHang_API.Dto;

namespace QLBanHang_API.Services.IService
{
    public interface IUserInfoService
    {
        Task<UserInfoDto> GetByUserName(string username);
        Task<UserInfoDto> UpdateUserInfo(Guid id, UpdateUserInfoDto userUpdate);
        Task<UserInfoDto> AddUserInfo( AddUserInfoDto userInfoDto);
        Task<UserInfoDto> GetUserById(Guid userId);
    }
}
