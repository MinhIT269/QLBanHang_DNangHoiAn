using AutoMapper;
using PBL6_QLBH.Models;
using QLBanHang_API.Repositories;
using QLBanHang_API.Request;
using QLBanHang_API.Services.IService;

namespace QLBanHang_API.Service
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;
		private readonly IImageService _imageService;
		public ProductService(IProductRepository productService, IMapper mapper, IImageService imageService)
		{
			_productRepository = productService;
			_mapper = mapper;
			_imageService = imageService;
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
									   // Loại bỏ khoảng trắng trong searchQuery nếu có
			searchQuery = searchQuery?.Trim() ?? string.Empty;
			var products = await _productRepository.GetProductsAsync(searchQuery, page, limit, sortCriteria, isDescending);

			return _mapper.Map<List<ProductDto>>(products);
		}

		public async Task<int> CountProductAsync(string searchQuery)
		{
			var totalProducts = string.IsNullOrEmpty(searchQuery)
				? await _productRepository.CountProductAsync()
				: (await _productRepository.FindProductsByNameAsync(searchQuery)).Count();
			var totalPages = (int)Math.Ceiling((double)totalProducts / 1);
			return totalPages;
		}
		public async Task AddProductAsync(ProductDto model, IFormFile mainImage, IList<IFormFile> additionalImages)
		{
			var imagePaths = new List<string>();
			if (mainImage != null & mainImage.Length > 0)
			{
				imagePaths.Add(await _imageService.UploadImageAsync(mainImage));
			}
			if (additionalImages != null)
			{
				foreach (var image in additionalImages.Where(img => img.Length > 0))
				{
					imagePaths.Add(await _imageService.UploadImageAsync(image));
				}
			}
			if (additionalImages != null)
			{
				foreach (var image in additionalImages.Where(img => img.Length > 0))
				{
					imagePaths.Add(await _imageService.UploadImageAsync(image));
				}
			}
			// Ánh xạ từ ProductDto sang Product
			var product = _mapper.Map<Product>(model);
			product.ImageUrl = imagePaths.FirstOrDefault(); // Cap nhat duong dan hinh anh
			product.ProductImages = imagePaths.Skip(1).Select(path => new ProductImage
			{
				ProductImageId = Guid.NewGuid(),
				ProductId = product.ProductId,
				ImageUrl = path
			}).ToList();

			await _productRepository.AddProductAsync(product);
		}

		public async Task UpdateProductAsync(ProductDto model, IFormFile? mainImage, IList<IFormFile>? additionalImages, List<string>? oldImageUrls)
		{
			// Lấy sản phẩm hiện tại từ CSDL
			var product = await _productRepository.GetProductByIdAsync(model.ProductId);
			if (product == null)
			{
				throw new KeyNotFoundException("Sản phẩm không tồn tại");
			}

			// Cập nhật thông tin cơ bản của sản phẩm
			_mapper.Map(model, product);  // Ánh xạ dữ liệu từ ProductDto sang Product

			// Xử lý ảnh chính nếu có ảnh mới
			if (mainImage != null && mainImage.Length > 0)
			{
				var mainImageUrl = await _imageService.UploadImageAsync(mainImage);
				product.ImageUrl = mainImageUrl;  // Cập nhật URL của ảnh chính
			}

			// Xử lý ảnh phụ
			var imageUrls = oldImageUrls != null ? new List<string>(oldImageUrls) : new List<string>();

			if (additionalImages != null && additionalImages.Count > 0)
			{
				foreach (var image in additionalImages)
				{
					var imageUrl = await _imageService.UploadImageAsync(image);
					imageUrls.Add(imageUrl);  // Thêm URL ảnh mới
				}
			}

			// Cập nhật danh sách ProductImages
			var productImages = imageUrls.Select(url => new ProductImage
			{
				ProductImageId = Guid.NewGuid(),
				ProductId = product.ProductId,
				ImageUrl = url
			}).ToList();

			// Ánh xạ ProductImages thành ProductImageDto
			model.ProductImages = _mapper.Map<List<ProductImageDto>>(productImages);

			// Gọi Repository để lưu thay đổi
			await _productRepository.UpdateProductAsync(product);
		}
		public async Task<bool> DeleteProductAsync(Guid id)
		{
			var result = await _productRepository.DeleteProductAsync(id);
			return result;
		}
	}
}
