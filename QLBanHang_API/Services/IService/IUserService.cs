using QLBanHang_API.Dto;

namespace QLBanHang_API.Services.IService
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsers();
        Task<UserDto> DeleteUser(string username);
        Task<UserDto> AddUser(AddUserDto user);
    }
}