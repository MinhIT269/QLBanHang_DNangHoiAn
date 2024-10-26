using PBL6_QLBH.Models;
using QLBanHang_API.Dto;

namespace QLBanHang_API.Services.IService
{
    public interface IUserInfoService
    {
        Task<UserInfoDto> GetByUserName(string userName);
        Task<UserInfoDto> UpdateUserInfo(string username, UpdateUserInfoDto userUpdate);
        Task<UserInfoDto> AddUserInfo(string username, UserInfoDto userInfoDto);
    }
}
