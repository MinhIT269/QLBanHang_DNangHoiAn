using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class ProductImage
    {
        [Key]
        public Guid ProductImageId { get; set; }
        public Guid ProductId { get; set; }

        [Required, MaxLength(255)]
        public string? ImageUrl { get; set; } // Đường dẫn đến ảnh chi tiết

        public Product? Product { get; set; }
    }
}
