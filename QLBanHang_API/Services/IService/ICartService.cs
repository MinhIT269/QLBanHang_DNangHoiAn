using QLBanHang_API.Dto;
using QLBanHang_API.Dto.Request;

namespace QLBanHang_API.Services.IService
{
	public interface ICartService
	{
        Task<List<CartItemDto>> GetAllCartItems(string userName);
        Task<CartItemDto> AddCartItem(CartItemRequest cartItemRequest);
        Task<List<CartItemDto>> DeleteCartItem(List<CartItemRequest> cartRequest);
        Task<List<CartItemDto>> UpdateCartItem(List<CartItemRequest> cartItems);
    }
}
