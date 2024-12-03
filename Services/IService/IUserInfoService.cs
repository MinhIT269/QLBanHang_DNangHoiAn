using PBL6.Dto;
using QLBanHang_API.Dto;

namespace PBL6.Services.IService
{
    public interface IUserInfoService
    {
        Task<UserInfoDto> GetByUserName(string userName);
        Task<UserInfoDto> UpdateUserInfo(string username, UpdateUserInfoDto userUpdate);
        Task<UserInfoDto> AddUserInfo(string username, AddUserInfoDto userInfoDto);
    }
}
