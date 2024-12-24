using Microsoft.EntityFrameworkCore;
using PBL6_QLBH.Data;
using PBL6_QLBH.DTOs;
using PBL6_QLBH.Models;
using QLBanHang_API.Repositories.IRepository;
using System.Runtime.CompilerServices;

namespace QLBanHang_API.Repositories.Repository
{
	public class CartRepository : ICartRepository
	{
        private readonly DataContext dataContext;
		public CartRepository(DataContext dataContext)
		{
			this.dataContext = dataContext;
		}
		
		public async Task<List<Cart>> GetCartItemDetails()
		{
			return await dataContext.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).ToListAsync();
		}

		public async Task<List<CartItem>> GetAllCartItemAsync(Guid userId)
		{
            var CartId = await dataContext.Carts
						.Where(x => x.User!.UserId == userId)
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
			foreach(var item in cartItems)
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
				if (cart != null)
				{
					dataContext.RemoveRange(cart);
                }
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
