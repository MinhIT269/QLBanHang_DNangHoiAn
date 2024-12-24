using PBL6.Dto;
using QLBanHang_API.Dto;

namespace PBL6.Services.IService
{
    public interface IUserInfoService
    {
        Task<UserInfoDto> GetByUserName(string username);
        Task<UserInfoDto> UpdateUserInfo(Guid id, UpdateUserInfoDto userUpdate);
        Task<UserInfoDto> AddUserInfo( AddUserInfoDto userInfoDto);
        Task<UserInfoDto> GetUserById(Guid userId);
        
    }
}
