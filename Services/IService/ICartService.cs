using PBL6.Dto;
using PBL6.Dto.Request;
using PBL6_QLBH.Models;

namespace PBL6.Services.IService
{
    public interface ICartService
    {
        Task<CartDto> AddProductToCart(Guid productId);

        Task<CartDto> GetCartOfUser(Guid userId);

        Task<List<CartItemDto>> GetAllCartItems(string userName);
        Task<CartItemDto> AddCartItem(CartItemRequest cartItemRequest);
        Task<List<CartItemDto>> DeleteCartItem(List<CartItemRequest> cartRequest);
        Task<List<CartItemDto>> UpdateCartItem(List<CartItemRequest> cartItems);
    }
}
