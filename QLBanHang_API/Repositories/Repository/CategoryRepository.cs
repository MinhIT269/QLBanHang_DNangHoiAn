using Microsoft.EntityFrameworkCore;
using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Repositories.Repository
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
        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
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
        public async Task<List<Category>> GetAllDetailCategory()
        {
            return await _dataContext.Categories.Include(c => c.ProductCategories)!.ThenInclude(p => p.Product).ToListAsync();
        }

        public IQueryable<Category> GetFilteredCategoriesQuery(string searchQuery, string sortCriteria, bool isDescending)
        {
            // Khởi tạo truy vấn với các Include cần thiết
            var query = _dataContext.Categories
                                    .Include(c => c.ProductCategories)!
                                    .ThenInclude(p => p.Product)
                                    .AsQueryable();

            // Áp dụng bộ lọc tìm kiếm nếu có
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => EF.Functions.Collate(c.CategoryName!, "SQL_Latin1_General_CP1_CI_AI").Contains(searchQuery));
            }

            // Áp dụng sắp xếp
            query = sortCriteria switch
            {
                "name" => isDescending ? query.OrderByDescending(c => c.CategoryName) : query.OrderBy(c => c.CategoryName),
                "productCount" => isDescending ? query.OrderByDescending(c => c.ProductCategories!.Sum(pc => pc.Product.Stock)) : query.OrderBy(c => c.ProductCategories.Sum(pc => pc.Product.Stock)),
                _ => query
            };

            return query;
        }

        public async Task<List<Product>> GetProductByCategoryIdAsync(Guid categoryId)
        {
            return await _dataContext.Products.Where(p => p.ProductCategories!.Any(pc => pc.CategoryId == categoryId)).ToListAsync();
        }

        public async Task<bool> IsCategoryNameExistsAsync(string categoryName)
        {
            return await _dataContext.Categories.AnyAsync(c => c.CategoryName == categoryName);
        }
    }
}
