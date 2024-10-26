using QLBanHang_API.Request;

namespace QLBanHang_API.Service
{
	public interface ICategoryService
	{
		Task<List<CategoryDto>> GetAllCategoriesAsync();
		Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
		Task CreateCategoryAsync(CategoryDto category);
		Task UpdateCategoryAsync(CategoryDto category);
		Task DeleteCategoryAsync(Guid id);
	}
}
