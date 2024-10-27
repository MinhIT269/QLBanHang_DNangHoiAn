using Microsoft.EntityFrameworkCore;
using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

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
        public async Task AddProductAsync(Product product)
        {
            _dataContext.Products.Add(product);
            await _dataContext.SaveChangesAsync();
        }
        public async Task UpdateProductAsync(Product product)
        {
            _dataContext.Products.Update(product);
            await _dataContext.SaveChangesAsync();
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

        public async Task<List<Product>> FindProductsByNameAsync(string name, int skip, int take)
        {

            var normalizedSearchTerm = name.Trim().ToLower();
            return await _dataContext.Products
                 .Include(p => p.ProductCategories!)
                    .ThenInclude(pc => pc.Category)
                 .Where(p => EF.Functions.Collate(p.Name!, "SQL_Latin1_General_CP1_CI_AI").Contains(normalizedSearchTerm))
                 .Skip(skip)
                 .Take(take)
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
    }
}
