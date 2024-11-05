using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto.Request;
using QLBanHang_API.Repositories;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Request;
using QLBanHang_API.Services.IService;
using System.Text.RegularExpressions;

namespace QLBanHang_API.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IProductImageRepository _productImageRepository;
        public ProductService(IProductRepository productService, IMapper mapper, IImageService imageService, IProductImageRepository productImageRepository)
        {
            _productRepository = productService;
            _mapper = mapper;
            _imageService = imageService;
            _productImageRepository = productImageRepository;
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

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return _mapper.Map<ProductDto>(product);    
        }

        public async Task<int> CountProductAsync(string searchQuery)
        {
            var totalProducts = string.IsNullOrEmpty(searchQuery)
                ? await _productRepository.CountProductAsync()
                : (await _productRepository.FindProductsByNameAsync(searchQuery)).Count();
            var totalPages = (int)Math.Ceiling((double)totalProducts / 10);
            return totalPages;
        }

        public async Task<bool> AddProductAsync(ProductRequest model, IFormFile mainImage, IList<IFormFile> additionalImages)
        {
            var imagePaths = new List<string>();

            if (mainImage != null & mainImage!.Length > 0)
            {
                imagePaths.Add(await _imageService.UploadImageAsync(mainImage, model.ProductId));
            }

            if (additionalImages != null)
            {
                foreach (var image in additionalImages.Where(img => img.Length > 0))
                {
                    imagePaths.Add(await _imageService.UploadImageAsync(image, model.ProductId));
                }
            }

			model.AdditionalImageUrls = imagePaths.Skip(1).ToList();
			// Ánh xạ từ ProductRequest sang Product
			var product = _mapper.Map<Product>(model);
            product.ImageUrl = imagePaths.FirstOrDefault(); // Cap nhat duong dan hinh anh

            return await _productRepository.AddProductAsync(product);
        }

        public async Task<bool> UpdateProductAsync(ProductRequest model, IFormFile? mainImage, IList<IFormFile>? additionalImages, List<string>? oldImageUrls)
        {
            var imagePaths = new List<string>();
            // Lấy sản phẩm hiện tại từ CSDL
            var product = await _productRepository.GetProductByIdAsync(model.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại");
            }

            _mapper.Map(model, product);  // Ánh xạ dữ liệu từ ProductRequest sang Product

            var allImageUrls = await _imageService.GetAllImageUrlsForProductAsync(model.ProductId);

            var oldImageSet = new HashSet<string>(oldImageUrls ?? new List<string>());  // Tạo một tập hợp các URL cũ để giữ lại

            var descriptionImageUrls = ExtractImageUrlsFromDescription(model.Description);
            foreach (var url in descriptionImageUrls)
            {
                    oldImageSet.Add(url); // Nếu chưa có thì thêm vào oldImageSet
            }

            foreach (var imageUrl in allImageUrls)
            {
                if (!oldImageSet.Contains(imageUrl))
                {
                    await _imageService.DeleteImageAsync(imageUrl); // Xóa ảnh nếu không nằm trong oldImageUrls
                }
                else if (!descriptionImageUrls.Contains(imageUrl))
                {
                    imagePaths.Add(imageUrl);
                }
            }
            if (mainImage != null && mainImage.Length > 0)    // Xử lý ảnh chính nếu có ảnh mới
            {
                var mainImageUrl = await _imageService.UploadImageAsync(mainImage, model.ProductId);
                product.ImageUrl = mainImageUrl;  // Cập nhật URL của ảnh chính
            }
            else
            {
                var firstOldImageUrl = oldImageUrls[0];

                // Xóa phần tử đầu tiên này nếu nó tồn tại trong imagePaths
                imagePaths.Remove(firstOldImageUrl);
            }

            if (additionalImages != null && additionalImages.Count > 0)
            {
                foreach (var image in additionalImages.Where(img => img.Length > 0))
                {
                    imagePaths.Add(await _imageService.UploadImageAsync(image, model.ProductId));
                }
            }

            if (product.ProductImages == null)
            {
                product.ProductImages = new List<ProductImage>();
            }
            foreach (var url in imagePaths)
            {
                product.ProductImages.Add(new ProductImage
                {
                    ProductImageId = Guid.NewGuid(),
                    ProductId = model.ProductId,
                    ImageUrl = url
                });
            }
            return await _productRepository.UpdateProductAsync(product);
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var result = await _productRepository.DeleteProductAsync(id);
            return result;
        }
        private List<string> ExtractImageUrlsFromDescription(string description)
        {
            var urls = new List<string>();
            var regex = new Regex("<img[^>]+?src=[\"'](?<url>.+?)[\"'][^>]*>", RegexOptions.IgnoreCase);

            var matches = regex.Matches(description);
            foreach (Match match in matches)
            {
                var url = match.Groups["url"].Value;
                urls.Add(url);
            }

            return urls;
        }
    }
}
