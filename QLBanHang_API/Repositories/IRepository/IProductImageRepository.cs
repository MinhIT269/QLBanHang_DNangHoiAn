using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface IProductImageRepository
    {
        Task<ProductImage?> GetByUrlAsync(string imageUrl);
        Task<bool> DeleteAsync(ProductImage productImage);
    }
}
