using QLBanHang_API.Request;

namespace QLBanHang_API.Service
{
	public interface IProductService
	{
		Task<List<ProductDto>> GetAllProductAsync();
		Task<List<ProductDto>> GetProductsAsync(string searchQuery, int page, int limit, string sortCriteria, bool isDescending);
		Task<int> CountProductAsync(string searchQuery);
		Task AddProductAsync(ProductDto model, IFormFile mainImage, IList<IFormFile> additionalImages);
		Task<bool> DeleteProductAsync(Guid id);
		Task UpdateProductAsync(ProductDto model, IFormFile? mainImage, IList<IFormFile>? additionalImages, List<string>? oldImageUrls);
	}
}
