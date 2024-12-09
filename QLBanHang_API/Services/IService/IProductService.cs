using QLBanHang_API.Dto.Request;
using QLBanHang_API.Request;

namespace QLBanHang_API.Service
{
	public interface IProductService
	{
		Task<List<ProductDto>> GetAllProductAsync();
		Task<List<ProductDto>> GetProductsAsync(string searchQuery, int page, int limit, string sortCriteria, bool isDescending);
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<int> CountProductAsync(string searchQuery);
		Task<bool> AddProductAsync(ProductRequest model, IFormFile mainImage, IList<IFormFile> additionalImages);
		Task<bool> DeleteProductAsync(Guid id);
		Task<bool> UpdateProductAsync(ProductRequest model, IFormFile? mainImage, IList<IFormFile>? additionalImages, List<string>? oldImageUrls);
		Task<List<ProductDto>> GetProductFromQuery(string? search, string? category, string? brandName, int page, bool isDescending);
	}
}
