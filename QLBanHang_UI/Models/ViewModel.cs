using System.Net;

namespace QLBanHang_UI.Models
{
    public class ViewModel
    {
        public List<ProductDto>? Products { get; set; }
        public List<CategoryDto>? Categorys { get; set; }
        public List<BrandDto>? Brands { get; set; }
        public List<CartItemDto>? Carts { get; set; }
    }
}
