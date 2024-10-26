using AutoMapper;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Services.IService;

namespace QLBanHang_API.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await userRepository.GetAllUserAsync();
            var usersDto = mapper.Map<List<UserDto>>(users);
            return usersDto;
        }

        public async Task<UserDto> DeleteUser(string username)
        {
            var user = await userRepository.DeleteUserAsync(username);
            var userDto= mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<UserDto> AddUser(AddUserDto userAdd)
        {
            var users = await userRepository.GetAllUserAsync();
            foreach ( var x in users)
            {
                if(userAdd.Username == x.Username)
                {
                    return null;
                }
            }
            var user = mapper.Map<User>(userAdd);
            var userDomain = await userRepository.AddUserAsync(user);
            var userDto = mapper.Map<UserDto>(userDomain);
            return userDto;
        }
    }
}
