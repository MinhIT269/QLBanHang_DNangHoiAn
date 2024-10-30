using QLBanHang_API.Request;

namespace QLBanHang_API.Service
{
	public interface ICategoryService
	{
		Task<List<CategoryDto>> GetAllCategoriesAsync();
		Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
		Task<bool> CreateCategoryAsync(CategoryDto category);
		Task<bool> UpdateCategoryAsync(CategoryDto category);
		Task<bool> DeleteCategoryAsync(Guid id);
	}
}
