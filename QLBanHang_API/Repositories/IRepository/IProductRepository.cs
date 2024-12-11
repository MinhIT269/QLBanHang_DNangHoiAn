using PBL6_QLBH.Models;
using QLBanHang_API.Request;
using System.Collections;

namespace QLBanHang_API.Repositories
{
	public interface IProductRepository
	{
		Task<List<Product>> GetAllProductAsync();
		Task<Product?> GetProductByIdAsync(Guid id);
		Task<List<Product>> GetProductsAsync(string searchQuery, int page, int limit, string sortCriteria, bool isDescending);
		Task<List<Product>> FindProductsAsync(string temp, Guid id);
		Task<List<Product>> FindProductsByNameAsync(string name);
		Task<int> CountProductAsync();
        Task<bool> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
		Task<bool> DeleteProductAsync(Guid id);
        Task<List<Product>> GetProductFromQueryAsync(string? search, string? category, string? brandName, int page, bool isDescending);
		Task<int> GetLowStockProducts();
		Task<int> GetNewProducts();
		Task<int> GetAvailableProduct();
	}

}
