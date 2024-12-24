using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface ICartRepository
    {
        public Task<Cart> AddProductToCart(Guid productId);

        public Task<Cart?> GetCartByUserIdAsync(Guid userId);

        Task<List<Cart>> GetCartItemDetails();
        Task<List<CartItem>> GetAllCartItemAsync(Guid userId);
        Task<List<CartItem>> GetAllCartItemAsync(string userName);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<List<CartItem>> DeleteCartItemAsync(List<CartItem> cartItems);
        Task<List<CartItem>> UpdateCartItemAsync(List<CartItem> cartItems);

    }
}
