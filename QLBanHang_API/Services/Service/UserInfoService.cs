using AutoMapper;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Services.IService;

namespace QLBanHang_API.Services.Service
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IUserInfoRepository userInfoRepository;
        private readonly IMapper mapper;
        public UserInfoService (IUserInfoRepository userInfoRepository, IMapper mapper)
        {
            this.userInfoRepository = userInfoRepository;
            this.mapper = mapper;
        }

        public async Task<UserInfoDto> GetByUserName(string userName)
        {
            var userInfo = await userInfoRepository.GetByUserNameAsync(userName);
            var userInfoDto = new UserInfoDto()
            {
                UserInfoId = userInfo.UserInfoId,
                Address = userInfo.Address,
                PhoneNumber = userInfo.PhoneNumber,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Gender = userInfo.Gender,
                user = new UserDto()
                {
                    UserName = userInfo.User.UserName,
                    Email = userInfo.User.Email,
                }
            };
            return userInfoDto;
        }

        public async Task<UserInfoDto> UpdateUserInfo(string username,UpdateUserInfoDto userUpdate)
        {
            var userInfo = await userInfoRepository.UpdateAsync(username, userUpdate);
            var userInfoDto = new UserInfoDto()
            {
                UserInfoId = userInfo.UserInfoId,
                Address = userInfo.Address,
                PhoneNumber = userInfo.PhoneNumber,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Gender = userInfo.Gender,
                user = new UserDto()
                {
                    UserName = userInfo.User.UserName,
                    Email = userInfo.User.Email,
                }
            };
            return userInfoDto;
        }

        public async Task<UserInfoDto> AddUserInfo(string username,AddUserInfoDto userInfoAddDto)
        {
            var userInfo = mapper.Map<UserInfo>(userInfoAddDto);
            var userInfoDomain = await userInfoRepository.AddUserInfoAsync(username, userInfo);
            var userInfoDto = new UserInfoDto()
            {
                UserInfoId = userInfoDomain.UserInfoId,
                Address = userInfoDomain.Address,
                PhoneNumber = userInfoDomain.PhoneNumber,
                FirstName = userInfoDomain.FirstName,
                LastName = userInfoDomain.LastName,
                Gender = userInfo.Gender,
                user = new UserDto()
                {
                    UserName = userInfoDomain.User.UserName,
                    Email = userInfoDomain.User.Email,
                }
            };
            return userInfoDto;
        }
    }
}
