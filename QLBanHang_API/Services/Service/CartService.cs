using AutoMapper;
using PBL6_QLBH.DTOs;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Dto.Request;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Services.IService;

namespace QLBanHang_API.Services.Service
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
		public async Task<List<CartItemDto>> GetAllCartItems(Guid userId)
		{
			var cartItems = await cartRepository.GetAllCartItemAsync(userId);
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
			var cartItemDomain = await  cartRepository.AddCartItemAsync(addCartItem);
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
			var updateCartItem = new List<CartItem>();
			foreach (var cartItem in cartItems)
			{
				var cartUpdate = new CartItem()
				{
					CartItemId = cartItem.CartItemId,
					ProductId = cartItem.ProductId,
					Quantity = cartItem.Quantity,
					CartId = cartItem.CartId,
				};
				updateCartItem.Add(cartUpdate);
			}
			var updateCartItemDomain = await cartRepository.UpdateCartItemAsync(updateCartItem);
			var updateCartItemsDto = mapper.Map<List<CartItemDto>>(updateCartItemDomain);
			return updateCartItemsDto;
		}
	}
}