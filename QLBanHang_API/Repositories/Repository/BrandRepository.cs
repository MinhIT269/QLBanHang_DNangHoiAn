using Microsoft.EntityFrameworkCore;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using QLBanHang_API.Repositories.IRepository;

namespace QLBanHang_API.Repositories.Repository
{
	public class BrandRepository : IBrandRepository
	{
		private readonly DataContext _dataContext;

		public BrandRepository(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		
		public async Task<List<Brand>> GetAllBrandsAsync()
		{
			return await _dataContext.Brands.ToListAsync();
		}
	}
}
