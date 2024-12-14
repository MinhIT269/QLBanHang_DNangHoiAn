using PBL6.Dto;
using PBL6.Dto.Request;
using PBL6_QLBH.Models;

namespace PBL6.Services.Service
{
    public interface IProductService
    {
        public IQueryable<ProductDto> getAllProduct();

        Task<List<ProductDto>> GetAllProductAsync();
        Task<List<ProductDto>> GetProductsAsync(string searchQuery, int page, int limit, string sortCriteria, bool isDescending);

        Task<List<ProductDto>> GetProductsByName(string productName, int skip, int size);

        Task<List<ProductDto>> GetProductsTrending( int skip, int size);

        Task<List<ProductDto>> GetProductsByCategory(string  categoryName, int skip,int size, bool getAll = false);

        Task<List<ProductDto>> GetProductNotYetReview(int skip,int size);


        Task<List<ProductDto>> GetProductsNew(int skip, int size);

        Task<List<ProductDto>> GetSuggestedProductsByCategory(Guid userId, int skip, int take);

        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<int> CountProductAsync(string searchQuery);
        Task<bool> AddProductAsync(ProductRequest model, IFormFile mainImage, IList<IFormFile> additionalImages);
        Task<bool> DeleteProductAsync(Guid id);
        Task<bool> UpdateProductAsync(ProductRequest model, IFormFile? mainImage, IList<IFormFile>? additionalImages, List<string>? oldImageUrls);
        Task<List<ProductDto>> GetProductFromQuery(string? search, string? category, string? brandName, int page, bool isDescending);
        Task<int> GetTotalProduct();
        Task<int> AvailableProducts();
        Task<int> GetLowStockProducts();
        Task<int> GetNewProducts();


    }
}
