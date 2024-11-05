using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QLBanHang_UI.Models
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }  // Khóa chính
        public string? Name { get; set; }
        public string? MetaTitle { get; set; }

        public string? Description { get; set; }

        public string? MetaDescription { get; set; }
        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; }
        public int Stock { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Warranty { get; set; }
        public string? BrandName { get; set; }
        public Guid? VideoId { get; set; }
        public List<string>? CategoryNames { get; set; }
        public List<string>? ProductImages { get; set; }
    }
}
