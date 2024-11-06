using Microsoft.EntityFrameworkCore;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using QLBanHang_API.Repositories.IRepository;

namespace QLBanHang_API.Repositories.Repository
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly DataContext _context;
        public ProductImageRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<ProductImage?> GetByUrlAsync(string url)
        {
            return await _context.ProductImages.FirstOrDefaultAsync(pi => pi.ImageUrl == url);
        }
        public async Task<bool> DeleteAsync(ProductImage productImage)
        {
            _context.ProductImages.Remove(productImage);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
