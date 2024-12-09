using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
	public interface ICartRepository
	{
        Task<List<Cart>> GetCartItemDetails();
        Task<List<CartItem>> GetAllCartItemAsync(string userName);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<List<CartItem>> DeleteCartItemAsync(List<CartItem> cartItems);
        Task<List<CartItem>> UpdateCartItemAsync(List<CartItem> cartItems);
    }
}
