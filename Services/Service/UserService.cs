using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PBL6.Dto;
using PBL6.Repositories.IRepository;
using PBL6.Services.IService;
using PBL6_QLBH.Models;

namespace PBL6.Services.Service
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper mapper;

        public UserService (IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }

        public async Task<ReviewDto> createReview(Review review)
        {
            var createdReview = await _userRepository.createReview(review);
            return mapper.Map<ReviewDto>(createdReview);
        }

        public async Task<List<UserAdminDto>> GetFilteredUsers(int page, int pageSize, string searchQuery, string sortCriteria, bool isDescending)
        {
            var query = _userRepository.GetFilteredUsers(searchQuery, sortCriteria, isDescending);

            var pagedUsers = await query.Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ProjectTo<UserAdminDto>(mapper.ConfigurationProvider)
                              .ToListAsync();
            return pagedUsers;
        }

        public async Task<User1Dto> GetUser(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
            return user;

        }
        public async Task<bool> UpdateUserInfoByUsernameAsync(string username, UpdateUserDto updatedInfo)
        {
            // Tìm User dựa trên Username
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null) return false;

            // Tìm UserInfo dựa trên UserId
            var userInfo = await _userRepository.GetUserInfoByUserIdAsync(user.UserId);
            if (userInfo == null) return false;

            // Cập nhật thông tin
            userInfo.Address = updatedInfo.Address ?? userInfo.Address;
            userInfo.PhoneNumber = updatedInfo.PhoneNumber ?? userInfo.PhoneNumber;
            userInfo.FirstName = updatedInfo.FirstName ?? userInfo.FirstName;
            userInfo.LastName = updatedInfo.LastName ?? userInfo.LastName;
            userInfo.Gender = updatedInfo.Gender;

            // Lưu thay đổi
            await _userRepository.UpdateUserInfoAsync(userInfo);
            return true;
        }
        public async Task<bool> UpdateUserAsync(string username, string password, string email)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return false; // Không tìm thấy user
            }

            // Cập nhật thông tin
            user.PasswordHash = password;
            user.Email = email;

            // Lưu thay đổi
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<int> GetTotalUserAsync(string searchQuery)
        {
            var query = _userRepository.GetFilteredUsers(searchQuery, "name", false);
            return await query.CountAsync();
        }


    }
}
