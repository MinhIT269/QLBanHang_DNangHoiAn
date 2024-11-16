using QLBanHang_API.Dto;
using QLBanHang_API.Request;

namespace QLBanHang_API.Service
{
	public interface ICategoryService
	{
		Task<List<CategoryDto>> GetAllCategoriesAsync();
		Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
        Task<List<CategoryDetailDto>> GetAllDetailCategory();
        Task<bool> CreateCategoryAsync(CategoryDto category);
		Task<bool> UpdateCategoryAsync(CategoryDto category);
		Task<bool> DeleteCategoryAsync(Guid id);
		Task<(List<CategoryDetailDto> categories, int totalRecords)> GetFilteredCategoriesAsync(int page, int pageSize, string searchQuery, string sortCriteria, bool isDescending);
        Task<int> GetTotalCategoriesAsync(string searchQuery);
		Task<List<ProductDto>> GetProductsByCategoryIdAsync(Guid categoryId);
		Task<bool> IsCategoryNameExistsAsync(string categoryName);

	}
}
