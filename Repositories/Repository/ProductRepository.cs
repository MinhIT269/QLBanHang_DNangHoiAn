using Microsoft.EntityFrameworkCore;
using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using System.ComponentModel.DataAnnotations;

namespace PBL6.Repositories.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _dataContext;

        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(DataContext dataContext, ILogger<ProductRepository> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<List<Product>> GetAllProductAsync()
        {
            return await _dataContext.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _dataContext.Products
                .Include(p => p.ProductCategories!) // Null-forgiving: Đảm bảo không null
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }
        public IQueryable<Product> GetAllProducts()
        {
            return _dataContext.Products
                .Include(p => p.ProductCategories!)
                .ThenInclude(pc => pc.Category);
        }
        public IQueryable<Product> FilterBySearchQuery(IQueryable<Product> query, string searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p => p.Name!.Contains(searchQuery));
            }
            return query;
        }
        public IQueryable<Product> ApplySorting(IQueryable<Product> query, string sortCriteria, bool isDescending)
        {
            switch (sortCriteria)
            {
                case "category":
                    return isDescending
                        ? query.OrderByDescending(p => p.ProductCategories!.OrderBy(pc => pc.Category!.CategoryName).FirstOrDefault()!.Category!.CategoryName)
                        : query.OrderBy(p => p.ProductCategories!.OrderBy(pc => pc.Category!.CategoryName).FirstOrDefault()!.Category!.CategoryName);
                case "stock":
                    return isDescending ? query.OrderByDescending(p => p.Stock) : query.OrderBy(p => p.Stock);
                case "name":
                    return isDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                case "price":
                    return isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                default:
                    return query;
            }
        }
        public IQueryable<Product> ApplyPaging(IQueryable<Product> query, int page, int limit)
        {
            return query.Skip((page - 1) * limit).Take(limit);
        }

        public async Task<List<Product>> GetProductsAsync(string searchQuery, int page, int limit, string sortCriteria, bool isDescending)
        {
            var query = GetAllProducts();
            query = FilterBySearchQuery(query, searchQuery);
            query = ApplySorting(query, sortCriteria, isDescending);
            query = ApplyPaging(query, page, limit);

            return await query.ToListAsync();
        }

        public async Task<List<Product>> FindProductsAsync(string temp, Guid id)
        {
            return await _dataContext.Products
                .Include(p => p.ProductCategories!)
                .ThenInclude(pc => pc.Category)
                .Where(p => p.Name!.ToLower().Contains(temp.ToLower()) &&
                            p.ProductCategories!.Any(pc => pc.CategoryId == id))
                .ToListAsync();
        }
      
        public async Task<int> CountProductAsync()
        {
            return await _dataContext.Products.CountAsync();
        }
        public async Task<bool> AddProductAsync(Product product)
        {
            _dataContext.Products.Add(product);
            int result = await _dataContext.SaveChangesAsync();
            return result > 0;
        }
        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                if (product.ProductImages != null)
                {
                    foreach (var image in product.ProductImages)
                    {
                        if (_dataContext.Entry(image).State == EntityState.Detached)
                        {
                            _dataContext.Entry(image).State = EntityState.Added;
                        }
                    }
                }
                _dataContext.Products.Update(product);
                int result = await _dataContext.SaveChangesAsync();
                return result > 0;
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                // Xử lý lỗi liên quan đến cơ sở dữ liệu
                Console.WriteLine($"Database Update Error: {dbEx.Message}");
                if (dbEx.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {dbEx.InnerException.Message}");
                }
                return false;
            }
            catch (ValidationException valEx)
            {
                // Xử lý lỗi xác thực dữ liệu
                Console.WriteLine($"Validation Error: {valEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _dataContext.Products.FindAsync(id);
            if (product != null)
            {
                _dataContext.Products.Remove(product);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Product>> GetProductsByNameAsync(string name, int skip, int take)
        {

            var normalizedSearchTerm = name.Trim().ToLower();
            return await _dataContext.Products
                 .Include(p => p.ProductCategories!)
                    .ThenInclude(pc => pc.Category)
                 .Include(p => p.Brand)
                     .ThenInclude(b => b.Locations)
                .Include(p => p.Video)
                 .Where(p => EF.Functions.Collate(p.Name!, "SQL_Latin1_General_CP1_CI_AI").Contains(normalizedSearchTerm))
                 .Skip(skip)
                 .Take(take)
                 .ToListAsync();
        }

        public async Task<List<Product>> FindProductsByNameAsync(string name)
        {
            var searchTerm = name.Trim();
            return await _dataContext.Products.Include(p => p.Brand)
                 .Include(p => p.ProductCategories!)
                 .ThenInclude(pc => pc.Category)
                 .Include(p => p.ProductImages)
                 .Where(p => EF.Functions.Collate(p.Name!, "SQL_Latin1_General_CP1_CI_AI").Contains(searchTerm))
                 .ToListAsync();
        }



        public async Task<List<Product>> GetTrendingProducts(int skip, int take)
        {
            try
            {
                var currentDate = DateTime.Now;
                var startDate = currentDate.AddDays(-7);

                var trendingProducts = _dataContext.OrderDetails
                    .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= currentDate)
                    .GroupBy(od => od.ProductId)
                    .Select(group => new
                    {
                        ProductId = group.Key,
                        TotalSold = group.Sum(od => od.Quantity)
                    })
                    .OrderByDescending(x => x.TotalSold)
                    .Take(100)
                    .ToList();

                var products = await _dataContext.Products
                    .Include(p => p.Brand)
                        .ThenInclude(b => b.Locations)
                    .Where(p => trendingProducts.Select(tp => tp.ProductId).Contains(p.ProductId))
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching trending products.");

                return new List<Product>();
            }
        }
        public async Task<List<Product>> GetProductsByCategory(string category, int skip = 0, int take = 0, bool getAll = false)
        {
            // Tìm CategoryId từ tên danh mục
            var categoryId = await _dataContext.Categories
                .Where(c => c.CategoryName == category)
                .Select(c => c.CategoryId)
                .FirstOrDefaultAsync();

            // Lấy danh sách ProductId thuộc Category
            var productIds = await _dataContext.ProductCategories
                .Where(pc => pc.CategoryId == categoryId)
                .Select(pc => pc.ProductId)
                .ToListAsync();

            if (productIds == null || productIds.Count == 0)
            {
                return new List<Product>();
            }

            // Lấy danh sách sản phẩm (bao gồm cả thông tin Brand và Locations)
            var query = _dataContext.Products
                .Include(p => p.Brand)
                .ThenInclude(b => b.Locations)
                .Where(p => productIds.Contains(p.ProductId));

            // Nếu không yêu cầu lấy tất cả, áp dụng phân trang
            if (!getAll)
            {
                query = query.Skip(skip).Take(take);
            }

            return await query.ToListAsync();
        }


        public async Task<List<Product>> GetProductNotYetReview(Guid id, int skip, int take)
        {

            if (skip < 0 || take <= 0)
            {
                throw new ArgumentException("Skip must be non-negative and take must be greater than zero.");
            }

            var completedOrders = await _dataContext.Orders
                .AsNoTracking()
                .Where(o => o.UserId == id && o.Status == "done")
                .Include(o => o.OrderDetails)
                .ToListAsync();

            if (!completedOrders.Any())
            {
                return new List<Product>();
            }


            var productIdsInOrders = completedOrders
               .SelectMany(o => o.OrderDetails)
               .Select(od => od.ProductId)
               .ToList();


            var reviewedProductIds = await _dataContext.Reviews
                 .Where(r => productIdsInOrders.Contains(r.ProductId)) 
                 .Select(r => r.ProductId)
                 .Distinct()
                 .ToListAsync();

            var notReviewedProductIds = productIdsInOrders
                .Except(reviewedProductIds) 
                .ToList();


            var productsNotReviewed = await _dataContext.Products
             .Where(p => notReviewedProductIds.Contains(p.ProductId))
             .Skip(skip)
             .Take(take)
             .ToListAsync();

            return productsNotReviewed;
        }

        public async Task<List<Product>> GetNewProducts(int skip, int take)
        {
            try
            {
                var newproducts = await _dataContext.Products
                    .OrderBy(p => p.CreatedDate)
                    .Include(p => p.Brand)
                        .ThenInclude(b => b.Locations)
                    .ToListAsync();


                var paginatedProducts = newproducts
                .Skip(skip)
                .Take(take)
                .ToList();
                return paginatedProducts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching new products.");

                return new List<Product>();
            }

        }

        public async Task<List<Product>> GetSuggestedProductsByCategory(Guid userId,int skip,int take)
        {
            var completedOrders = await _dataContext.Orders
              .Where(o => o.Status == "done" 
              && o.UserId == userId 
              && o.OrderDate >= DateTime.Now.AddDays(-7)) 
              .Include(o => o.OrderDetails)
              .ThenInclude(od => od.Product)
               .ThenInclude(p => p.ProductCategories)  // Bao gồm thông tin về ProductCategories
        .ThenInclude(pc => pc.Category)
             .AsSplitQuery()
              .ToListAsync();

            var categoryIds = await _dataContext.Orders
     .Where(o => o.Status == "done" && o.UserId == userId && o.OrderDate >= DateTime.Now.AddDays(-7))
     .SelectMany(o => o.OrderDetails)
     .Select(od => od.Product.ProductCategories)
     .SelectMany(pc => pc.Select(p => p.CategoryId))
     .Distinct()
     .ToListAsync();


            return await _dataContext.Products
              .Where(p => p.ProductCategories.Any(pc => categoryIds.Contains(pc.CategoryId)))
              .Skip(skip)
              .Take(take)
              .ToListAsync();
        }


        public async Task<int> GetAvailableProduct()
        {
            var product = await _dataContext.Products.Where(p => p.Stock >= 10).CountAsync();
            return product;
        }

        public async Task<int> GetLowStockProducts()
        {
            var product = await _dataContext.Products.Where(p => p.Stock < 10).CountAsync();
            return product;
        }

        public async Task<int> GetNewProducts()
        {
            var recentDate = DateTime.Now.AddDays(-3);  // Lấy sản phẩm mới trong 
            var newProducts = await _dataContext.Products.Where(p => p.CreatedDate >= recentDate).ToListAsync();

            return newProducts.Count;
        }
    }
}
