using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface ICartRepository
    {
        public Task<Cart> AddProductToCart(Guid productId);

        public Task<Cart?> GetCartByUserIdAsync(Guid userId);

    }
}
