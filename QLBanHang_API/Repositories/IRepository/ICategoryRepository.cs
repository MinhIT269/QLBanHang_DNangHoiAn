using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
	public interface ICategoryRepository
	{
		Task<List<Category>> GetAllCategoriesAsync();
		Task<Category?> GetCategoryByIdAsync(Guid id);
		Task CreateCategoryAsync(Category category);
		Task UpdateCategoryAsync(Category category);
		Task DeleteCategoryAsync(Guid id);
	}
}
