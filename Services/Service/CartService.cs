using AutoMapper;
using PBL6.Dto;
using PBL6.Repositories.IRepository;
using PBL6.Services.IService;

namespace PBL6.Services.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async  Task<CartDto> AddProductToCart(Guid productId)
        {
            var cart = await _repository.AddProductToCart(productId);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> GetCartOfUser(Guid userId)
        {
            var cart = await _repository.GetCartByUserIdAsync(userId);

            return _mapper.Map<CartDto>(cart);
        }
    }
}
