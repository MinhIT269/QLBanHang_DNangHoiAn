using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Repositories.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext dataContext;
        private readonly IProductRepository _productRepository;

        public CartRepository(DataContext dataContext , IProductRepository productRepository)
        {
            this.dataContext = dataContext;
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
                dataContext.Entry(cartItem).State = EntityState.Modified;
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
                dataContext.CartItems.Add(newCartItem);


            }
            dataContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await dataContext.SaveChangesAsync();
            dataContext.ChangeTracker.AutoDetectChangesEnabled = true;


            return cart;
        }

        public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
        {
            return await dataContext.Carts
              .Include(c => c.CartItems)
                .ThenInclude(c => c.Product)
              .FirstOrDefaultAsync(c => c.UserId == userId);
        }


        public async Task<List<Cart>> GetCartItemDetails()
        {
            return await dataContext.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).ToListAsync();
        }

        public async Task<List<CartItem>> GetAllCartItemAsync(string userName)
        {
            var CartId = await dataContext.Carts
                        .Where(x => x.User.UserName == userName)
                        .Select(x => x.CartId)
                        .FirstOrDefaultAsync();

            var cartItems = await dataContext.CartItems
                                .Include(x => x.Product)
                                .Where(p => p.CartId == CartId).ToListAsync();
            return cartItems;
        }

        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            await dataContext.CartItems.AddAsync(cartItem);
            await dataContext.SaveChangesAsync();
            return cartItem;
        }

        public async Task<List<CartItem>> DeleteCartItemAsync(List<CartItem> cartItems)
        {
            var deleteItems = new List<CartItem>();
            foreach (var item in cartItems)
            {
                var deleteItem = await dataContext.CartItems.FirstOrDefaultAsync(x => x.CartItemId == item.CartItemId);
                if (deleteItem != null)
                {
                    dataContext.CartItems.Remove(deleteItem);
                    deleteItems.Add(deleteItem);
                }
            }
            await dataContext.SaveChangesAsync();
            return deleteItems;
        }
        public async Task<List<CartItem>> UpdateCartItemAsync(List<CartItem> cartItems)
        {
            try
            {
                var cartId = cartItems.First().CartId;
                var cart = dataContext.CartItems.Where(x => x.CartId == cartId).ToList();
                if (cart == null)
                {
                    return null;
                }
                dataContext.RemoveRange(cart);
                await dataContext.AddRangeAsync(cartItems);
                await dataContext.SaveChangesAsync();
                return cartItems;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Có lỗi khi cập nhật giỏ hàng.", ex);
            }

        }
    }
}
