using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllBrandAsync();
        Task<Brand> AddBrandAsync(Brand brand);
        Task<Brand> UpdateBrandAsync(Brand brand);
        Task<Brand> GetBrandByNameAsync(string brandName);
        Task<Brand> DeleteBrandAsync(Guid id);
    }
}
