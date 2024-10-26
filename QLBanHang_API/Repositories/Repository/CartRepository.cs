using Microsoft.EntityFrameworkCore;
using PBL6_QLBH.Data;
using PBL6_QLBH.DTOs;
using PBL6_QLBH.Models;
using QLBanHang_API.Repositories.IRepository;

namespace QLBanHang_API.Repositories.Repository
{
	public class CartRepository : ICartRepository
	{
        private readonly DataContext _dataContext;
		public CartRepository(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		
		public async Task<List<Cart>> GetCartItemDetails()
		{
			return await _dataContext.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).ToListAsync();
		}
	}
}
