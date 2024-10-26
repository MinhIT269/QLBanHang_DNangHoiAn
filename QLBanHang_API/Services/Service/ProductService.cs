using AutoMapper;
using QLBanHang_API.Repositories;
using QLBanHang_API.Request;

namespace QLBanHang_API.Service
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;

		public ProductService(IProductRepository productService, IMapper mapper)
		{
			_productRepository = productService;
			_mapper = mapper;
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
	}
}
