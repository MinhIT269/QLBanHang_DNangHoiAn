using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Repositories.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _dataContext;
        private readonly IProductRepository _productRepository;

        public CartRepository(DataContext dataContext , IProductRepository productRepository)
        {
            _dataContext = dataContext;
            _productRepository = productRepository; 
        }

        public async Task<Cart> AddProductToCart(Guid productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                throw new ArgumentException("Invalid product ID.");
            }

            var cart = await GetCartByUserIdAsync(Guid.Parse("d4e56743-ff2c-41d3-957d-576e9f574c5d"));
            Console.WriteLine("cart Id:" + cart.CartId);


            if (cart == null)
            {
                throw new ArgumentException("Cart not found");
            }


            if (cart.CartItems == null)
            {
                Console.WriteLine("cartItem List not intit");
                cart.CartItems = new List<CartItem>();
            }


            var cartItem = cart.CartItems?.FirstOrDefault(item => item.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += 1;
                _dataContext.Entry(cartItem).State = EntityState.Modified;
            }
            else
            {
                Console.WriteLine("cartItem not intit");
                CartItem newCartItem = new CartItem
                {
                    CartItemId = Guid.NewGuid(),
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = 1,
                };

                cart.CartItems?.Add(newCartItem);
                _dataContext.CartItems.Add(newCartItem);


            }
            _dataContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await _dataContext.SaveChangesAsync();
            _dataContext.ChangeTracker.AutoDetectChangesEnabled = true;


            return cart;
        }

        public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
        {
            return await _dataContext.Carts
              .Include(c => c.CartItems)
                .ThenInclude(c => c.Product)
              .FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
