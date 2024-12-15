using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(Guid id);
        Task<bool> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(Guid id);
        Task<List<Category>> GetAllDetailCategory();
        IQueryable<Category> GetFilteredCategoriesQuery(string searchQuery, string sortCriteria, bool isDescending);
        Task<List<Product>> GetProductByCategoryIdAsync(Guid categoryId);
        Task<bool> IsCategoryNameExistsAsync(string categoryName);

    }
}
