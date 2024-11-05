
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
            return await dbContext.Brands.ToListAsync();
        }
        //Add Async 
        public async Task<Brand> AddBrandAsync(Brand brand)
        {
            //Nếu ko trùng thoát vòng lặp
            while (await dbContext.Brands.AnyAsync(x=>x.BrandId == brand.BrandId))
            {
                brand.BrandId = Guid.NewGuid();
            }
            await dbContext.Brands.AddAsync(brand);
            await dbContext.SaveChangesAsync();
            return brand;
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
    }
}
