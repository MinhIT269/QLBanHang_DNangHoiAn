using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductAsync();
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<List<Product>> GetProductsAsync(string searchQuery, int page, int limit, string sortCriteria, bool isDescending);
        Task<List<Product>> FindProductsAsync(string temp, Guid id);
        Task<List<Product>> GetProductsByNameAsync(string name, int skip, int take);
        Task<int> CountProductAsync();
        Task<List<Product>> FindProductsByNameAsync(string name);
        Task<bool> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(Guid id);

        Task<List<Product>> GetTrendingProducts(int skip, int take);

        Task<List<Product>> GetProductsByCategory(string category, int skip = 0, int take = 0, bool getAll = false);
        Task<List<Product>> GetProductNotYetReview(Guid id, int skip, int take);

        Task<List<Product>> GetNewProducts(int skip, int take);

        Task<List<Product>> GetSuggestedProductsByCategory(Guid userId,int skip,int take);
        Task<List<Product>> GetProductFromQueryAsync(string? search, string? category, string? brandName, int page, bool isDescending);

        Task<int> GetLowStockProducts();
        Task<int> GetNewProducts();
        Task<int> GetAvailableProduct();

    }
}
