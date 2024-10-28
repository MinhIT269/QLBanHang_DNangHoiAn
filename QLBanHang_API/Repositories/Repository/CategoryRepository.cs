using Microsoft.EntityFrameworkCore;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using QLBanHang_API.Repositories.IRepository;

namespace QLBanHang_API.Repositories.Repository
{
	public class CategoryRepository : ICategoryRepository
	{
		public readonly DataContext _dataContext;
		public CategoryRepository(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<List<Category>> GetAllCategoriesAsync()
		{
			return await _dataContext.Categories.ToListAsync();
		}
		public async Task<Category?> GetCategoryByIdAsync(Guid id)
		{
			return await _dataContext.Categories.FindAsync(id);
		}
		public async Task<bool> CreateCategoryAsync(Category category)
		{
			_dataContext.Categories.Add(category);
            int result = await _dataContext.SaveChangesAsync();
			return result > 0;
		}
		public async Task<bool> UpdateCategoryAsync(Category category)
		{
			_dataContext.Categories.Update(category);
            int result = await _dataContext.SaveChangesAsync();
            return result > 0;
        }
		public async Task<bool>  DeleteCategoryAsync(Guid categoryId)
		{
			var category = await GetCategoryByIdAsync(categoryId);
			if (category != null)
			{
				_dataContext.Categories.Remove(category);
                int result = await _dataContext.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }
	}
}
