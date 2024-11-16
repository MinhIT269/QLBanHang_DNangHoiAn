using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllBrandAsync();
        Task<Brand> AddBrandAsync(Brand brand);
        Task<Brand> UpdateBrandAsync(Guid id, Brand brand);
        Task<Brand> GetBrandByNameAsync(string brandName);
        Task<Brand?> GetBrandByIdAsync(Guid id);
		Task<Brand> DeleteBrandAsync(Guid id);
        Task<List<Brand>> GetAllDetailBrand();
        IQueryable<Brand> GetFilteredBrandsQuery(string searchQuery, string sortCriteria, bool isDescending);
        Task<bool> HasProductsByBrandIdAsync(Guid brandId);
		Task<bool> IsBrandNameExists(string brandName);
	}
}
