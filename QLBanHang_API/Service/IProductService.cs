using QLBanHang_API.Request;

namespace QLBanHang_API.Service
{
	public interface IProductService
	{
		Task<List<ProductDto>> GetAllProductAsync();
		Task<List<ProductDto>> GetProductsAsync(string searchQuery, int page, int limit, string sortCriteria, bool isDescending);
	}
}
