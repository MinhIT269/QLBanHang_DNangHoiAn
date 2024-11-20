using PBL6.Dto;
using PBL6_QLBH.Models;

namespace PBL6.Services.Service
{
    public interface IProductService
    {
        public IQueryable<Product> getAllProduct();

        Task<List<ProductDto>> GetAllProductAsync();
        Task<List<ProductDto>> GetProductsAsync(string searchQuery, int page, int limit, string sortCriteria, bool isDescending);

        Task<List<ProductDto>> GetProductsByName(string productName, int skip, int size);

        Task<List<ProductDto>> GetProductsTrending( int skip, int size);

        Task<List<ProductDto>> GetProductsByCategory(string  categoryName, int skip,int size);

        Task<List<ProductDto>> GetProductNotYetReview(int skip,int size);


        Task<List<ProductDto>> GetProductsNew(int skip, int size);

    }
}
