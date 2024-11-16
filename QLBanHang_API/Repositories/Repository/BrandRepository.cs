
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using QLBanHang_API.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
namespace QLBanHang_API.Repositories.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly DataContext dbContext;
        public BrandRepository(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }
        //GetAll 
        public async Task<List<Brand>> GetAllBrandAsync()
        {
            return await dbContext.Brands.Include( b=> b.Locations).ToListAsync();
        }

        //Add Async 
        public async Task<Brand> AddBrandAsync(Brand brand)
        {
            //Nếu ko trùng thoát vòng lặp
            while (await dbContext.Brands.AnyAsync(x=>x.BrandId == brand.BrandId))
            {
                brand.BrandId = Guid.NewGuid();
            }

            if (brand.Locations != null && brand.Locations.Any())
            {
                foreach (var location in brand.Locations)
                {
                    location.BrandId = brand.BrandId;
                }
            }
            await dbContext.Brands.AddAsync(brand);
            await dbContext.SaveChangesAsync();
            return brand;
        }

        //Get Brand async
        public async Task<Brand> GetBrandByNameAsync(string brandName)
        {
            brandName = brandName.Trim().ToLower();
            var brands = dbContext.Brands.Include("Locations").AsQueryable();
            var brand = await brands.Where(p => EF.Functions.Collate(p.BrandName!, "SQL_Latin1_General_CP1_CI_AI").Contains(brandName)).FirstOrDefaultAsync();
            if (brand == null)
            {
                return null;
            }
            return brand;
        }

        // Get Brand by Id
        public async Task<Brand?> GetBrandByIdAsync(Guid id)
        {
            return await dbContext.Brands.Include(b=>b.Locations).FirstOrDefaultAsync(b => b.BrandId == id);
        }

        // Delete Brand Async
        public async Task<Brand> DeleteBrandAsync(Guid id)
        {
            var brand = await dbContext.Brands.FirstOrDefaultAsync(x => x.BrandId == id);
            if (brand == null)
            {
                return null;
            }
            dbContext.Brands.Remove(brand);
            await dbContext.SaveChangesAsync();
            return brand;
        }
        public async Task<List<Brand>> GetAllDetailBrand()
        {
            return await dbContext.Brands.Include(b => b.Products).Include(b => b.Locations).ToListAsync();
        }

        public IQueryable<Brand> GetFilteredBrandsQuery(string searchQuery, string sortCriteria, bool isDescending)
        {
            var query = dbContext.Brands
                .Include(b => b.Products)
                .Include(b => b.Locations)
                .AsQueryable();

            // Áp dụng bộ lọc tìm kiếm nếu có
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(b =>
                    EF.Functions.Collate(b.BrandName, "SQL_Latin1_General_CP1_CI_AI")!.Contains(searchQuery) ||
                    EF.Functions.Collate(b.Description, "SQL_Latin1_General_CP1_CI_AI")!.Contains(searchQuery)
                );
            }

            // Áp dụng sắp xếp
            query = sortCriteria switch
            {
                "name" => isDescending
                    ? query.OrderByDescending(b => b.BrandName)
                    : query.OrderBy(b => b.BrandName),

                "productCount" => isDescending
                    ? query.OrderByDescending(b => b.Products!.Sum(p => p.Stock))
                    : query.OrderBy(b => b.Products!.Sum(p => p.Stock)),

                _ => query
            };

            return query;
        }
		public async Task<bool> IsBrandNameExists(string brandName)
		{
			return await dbContext.Brands.AnyAsync(b => b.BrandName == brandName);
		}

		//Update async
		public async Task<Brand> UpdateBrandAsync(Guid id, Brand brandUpdate)
		{
			var brand = await dbContext.Brands.FirstOrDefaultAsync(x => x.BrandId == id);
			if (brand == null)
			{
				return null;
			}
			brand.BrandName = brandUpdate.BrandName;
			brand.Description = brandUpdate.Description;
			await dbContext.SaveChangesAsync();
			return brand;
		}

		public async Task<bool> HasProductsByBrandIdAsync(Guid brandId)
		{
			return await dbContext.Products.AnyAsync(p => p.BrandId == brandId);
		}

	}
}
