using Microsoft.EntityFrameworkCore;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using QLBanHang_API.Request;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace QLBanHang_API.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly DataContext _dataContext;
		public ProductRepository(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<List<Product>> GetAllProductAsync()
		{
			return await _dataContext.Products
				.Include(p => p.Brand)
				.Include(p => p.ProductCategories!) 
					.ThenInclude(pc => pc.Category)
				.Include(p => p.ProductImages).ToListAsync();
		}

		public async Task<Product?> GetProductByIdAsync(Guid id)
		{
			//trả về null nếu ko có đối tượng 
			return await _dataContext.Products
				.Include(p => p.Brand)
				.Include(p => p.ProductCategories!) // Null-forgiving: Đảm bảo không null
					.ThenInclude(pc => pc.Category)
				.Include(p => p.ProductImages)
				.Include(p => p.Reviews)
				.FirstOrDefaultAsync(p => p.ProductId == id);
		}
		public IQueryable<Product> GetAllProducts()
		{
			return _dataContext.Products.Include(p => p.Brand)
				.Include(p => p.ProductCategories!)
				.ThenInclude(pc => pc.Category)
				.Include(p => p.ProductImages);
		}
		public IQueryable<Product> FilterBySearchQuery(IQueryable<Product> query, string searchQuery)
		{
			var name = searchQuery.Trim();
			if (!string.IsNullOrEmpty(searchQuery))
			{
				query = query.Where(p => EF.Functions.Collate(p.Name!, "SQL_Latin1_General_CP1_CI_AI").Contains(name));
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

		public async Task<List<Product>> FindProductsAsync(string name, Guid id)
		{
            var searchTerm = name.Trim();
            return await _dataContext.Products.Include(p => p.Brand)
				.Include(p => p.ProductCategories!)
				.ThenInclude(pc => pc.Category)
				.Include(p => p.ProductImages)
				.Where(p => EF.Functions.Collate(p.Name!, "SQL_Latin1_General_CP1_CI_AI").Contains(searchTerm) &&
							p.ProductCategories!.Any(pc => pc.CategoryId == id))
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
        public async Task<List<Product>> GetProductFromQueryAsync(string? search, string? category, string? brandName, int page, bool isDescending)
		{
			var products = GetAllProducts();
			if (!string.IsNullOrEmpty(search))
			{
				products = products.Where(p => EF.Functions.Collate(p.Name!, "SQL_Latin1_General_CP1_CI_AI").Contains(search.Trim()));
			}

			if (!string.IsNullOrEmpty(category))
			{
				products = products.Include(p=> p.ProductCategories!).ThenInclude(p=> p.Category!.CategoryName)
					.Where(p=> p.ProductCategories!.FirstOrDefault()!.Category!.CategoryName == category);
			}
			if (!string.IsNullOrEmpty(brandName))
			{
                products = products.Where(p => EF.Functions.Collate(p.Brand!.BrandName!, "SQL_Latin1_General_CP1_CI_AI").Contains(brandName.Trim()));
            }

			var skipResult = (page - 1) * 8;
			return await products.Skip(skipResult).Take(8).ToListAsync();
		}
		public async Task<int> GetAvailableProduct()
		{ 
			var product = await _dataContext.Products.Where( p => p.Stock >= 10).CountAsync();
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
