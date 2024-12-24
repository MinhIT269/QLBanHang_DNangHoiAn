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
                UserInfoId = userInfo.UserId,
                Address = userInfo.Address,
                PhoneNumber = userInfo.PhoneNumber,
                Email = userInfo.User!.Email!,
                UserName = userName,
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


        public async Task<UserInfoDto> UpdateUserInfo(Guid id,UpdateUserInfoDto userUpdate)
        {
            var userInfo = await userInfoRepository.UpdateAsync(id, userUpdate);
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

        public async Task<UserInfoDto> AddUserInfo(AddUserInfoDto userInfoAddDto)
        {
            var userInfo = mapper.Map<UserInfo>(userInfoAddDto);
            var userInfoDomain = await userInfoRepository.AddUserInfoAsync(userInfo);
            var userInfoDto = new UserInfoDto()
            {
                Address = userInfoDomain.Address,
                PhoneNumber = userInfoDomain.PhoneNumber,
                FirstName = userInfoDomain.FirstName,
                LastName = userInfoDomain.LastName,
            };
            return userInfoDto;
        }

        public async Task<UserInfoDto> GetUserById(Guid userId)
        {
            var userInfo = await userInfoRepository.GetByUserId(userId);
            if(userInfo == null)
            {
                return null;
            }
            var userInfoDto = new UserInfoDto()
            {
                UserInfoId = userInfo.UserId,
                Address = userInfo.Address,
                PhoneNumber = userInfo.PhoneNumber,
                Email = userInfo.User!.Email!,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Gender = userInfo.Gender,
            };
            return userInfoDto;
        }
    }
}
