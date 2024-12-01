using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QLBanHang_API.Dto;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Repositories.Repository;
using QLBanHang_API.Services.IService;

namespace QLBanHang_API.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            this.mapper = mapper;
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
    }
}
