using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface IBrandRepository
    {
        Task<Brand> AddBrandAsync(Brand brand);
        Task<Brand> UpdateBrandAsync(Guid id, Brand brand);
        Task<Brand> GetBrandByNameAsync(string brandName);
        Task<Brand> DeleteBrandAsync(Guid id);
    }
}
