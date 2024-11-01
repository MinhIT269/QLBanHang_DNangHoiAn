using AutoMapper;
using PBL6.Dto;
using PBL6.Repositories.IRepository;
using PBL6.Services.IService;
using PBL6_QLBH.Models;

namespace PBL6.Services.Service
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService (IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ReviewDto> createReview(Review review)
        {
            var createdReview = await _userRepository.createReview(review);
            return _mapper.Map<ReviewDto>(createdReview);
        }

    }
}
