﻿using Microsoft.EntityFrameworkCore;
using PBL6.Dto;
using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Repositories.Repository
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly DataContext dbContext;
        public UserInfoRepository(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }


        // Get UserInfo By ID
        public async Task<UserInfo> GetByUserNameAsync(string username)
        {
            var userInfo = await dbContext.UserInfos.Include("User").FirstOrDefaultAsync(x => x.User.UserName == username);
            if (userInfo == null)
            {
                return null;
            }
            return userInfo;
        }

        //Update UserInfo
        public async Task<UserInfo> UpdateAsync(string? username, UpdateUserInfoDto userUpdate)
        {
            var userInfo = await dbContext.UserInfos
                .Include(u => u.User) // Tải đối tượng User
                .FirstOrDefaultAsync(x => x.User.UserName == username);

            // Kiểm tra xem userInfo có null hay không
            if (userInfo == null)
            {
                return null; // Trả về null nếu không tìm thấy
            }

            // Cập nhật các thuộc tính
            userInfo.FirstName = userUpdate.FirstName;
            userInfo.LastName = userUpdate.LastName;
            userInfo.PhoneNumber = userUpdate.PhoneNumber;
            userInfo.Address = userUpdate.Address;
            userInfo.Gender = (bool)userUpdate.Gender;

            // Kiểm tra User trước khi gán Email
            if (userInfo.User != null)
            {
                userInfo.User.Email = userUpdate.Email;
            }

            await dbContext.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            return userInfo; // Trả về đối tượng đã cập nhật
        }

        //Add
        public async Task<UserInfo> AddUserInfoAsync(UserInfo userInfo)
        {
            userInfo.UserInfoId = Guid.NewGuid();
            await dbContext.UserInfos.AddAsync(userInfo);
            await dbContext.SaveChangesAsync();
            return userInfo;
        }
    }
}
