using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface IProductImageRepository
    {
        Task<ProductImage?> GetByUrlAsync(string imageUrl);
        Task<bool> DeleteAsync(ProductImage productImage);
    }
}
