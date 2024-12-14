using AutoMapper;
using PBL6.Dto;
using PBL6.Dto.Request;
using PBL6.Repositories.IRepository;
using PBL6.Repositories.Repository;
using PBL6.Services.IService;
using PBL6_QLBH.Models;

namespace PBL6.Services.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;
        private readonly IMapper mapper;

        public CartService(ICartRepository cartRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.cartRepository = cartRepository;
        }

        public async  Task<CartDto> AddProductToCart(Guid productId)
        {
            var cart = await cartRepository.AddProductToCart(productId);
            return mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> GetCartOfUser(Guid userId)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId);

            return mapper.Map<CartDto>(cart);
        }

        public async Task<List<CartItemDto>> GetAllCartItems(string userName)
        {
            var cartItems = await cartRepository.GetAllCartItemAsync(userName);
            var cartItemsDto = mapper.Map<List<CartItemDto>>(cartItems);
            return cartItemsDto;
        }

        public async Task<CartItemDto> AddCartItem(CartItemRequest cartItemRequest)
        {
            //var addCartItem = mapper.Map<CartItem>(cartItemRequest);
            var addCartItem = new CartItem()
            {
                CartId = cartItemRequest.CartId,
                //CartItemId = cartItemRequest.CartItemId,
                ProductId = cartItemRequest.ProductId,
                Quantity = cartItemRequest.Quantity,
            };
            var cartItemDomain = await cartRepository.AddCartItemAsync(addCartItem);
            var cartItemDto = mapper.Map<CartItemDto>(cartItemDomain);
            return cartItemDto;
        }

        public async Task<List<CartItemDto>> DeleteCartItem(List<CartItemRequest> cartRequest)
        {
            var deleteCartItem = mapper.Map<List<CartItem>>(cartRequest);
            var deleteCartItemDomain = await cartRepository.DeleteCartItemAsync(deleteCartItem);
            var deleteCartItemsDto = mapper.Map<List<CartItemDto>>(deleteCartItemDomain);
            return deleteCartItemsDto;
        }
        public async Task<List<CartItemDto>> UpdateCartItem(List<CartItemRequest> cartItems)
        {
            var updateCartItem = mapper.Map<List<CartItem>>(cartItems);
            var updateCartItemDomain = await cartRepository.UpdateCartItemAsync(updateCartItem);
            var updateCartItemsDto = mapper.Map<List<CartItemDto>>(updateCartItemDomain);
            return updateCartItemsDto;
        }
    }
}
