using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PBL6.Dto;
using PBL6.Repositories.IRepository;
using PBL6.Services.Service;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Services.ServiceImpl
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(DataContext context,IMapper mapper, IProductRepository productRepository)
        {
            _context = context;
            _mapper = mapper;
            _productRepository = productRepository;
        }
        public IQueryable<Product> getAllProduct()
        {
            return _context.Products.AsQueryable(); // Trả về IQueryable để áp dụng Skip và Take
        }

        public async Task<List<ProductDto>> GetAllProductAsync()
        {
            var products = await _productRepository.GetAllProductAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<List<ProductDto>> GetProductsAsync(string searchQuery, int page, int limit, string sortCriteria, bool isDescending)
        {
            if (page < 1) page = 1;
            if (limit < 1) limit = 10; // Giá trị mặc định cho limit


            var products = await _productRepository.GetProductsAsync(searchQuery, page, limit, sortCriteria, isDescending);

            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<List<ProductDto>> GetProductsByCategory(string categoryName, int skip, int size)
        {
            var products = await _productRepository.GetProductsByCategory(categoryName, skip, size);
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<List<ProductDto>> GetProductsByName(string productName, int skip, int size)
        {
            var products = await _productRepository.FindProductsByNameAsync(productName,skip,size);
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<List<ProductDto>> GetProductsTrending(int skip, int size)
        {
            var products = await _productRepository.GetTrendingProducts(skip,size);
            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}
