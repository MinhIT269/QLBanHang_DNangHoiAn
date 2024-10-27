using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
	public interface ICategoryRepository
	{
		Task<List<Category>> GetAllCategoriesAsync();
		Task<Category?> GetCategoryByIdAsync(Guid id);
		Task<bool> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
		Task<bool> DeleteCategoryAsync(Guid id);
	}
}
