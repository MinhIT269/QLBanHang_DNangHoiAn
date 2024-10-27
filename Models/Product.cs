using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PBL6_QLBH.Models
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }  // Khóa chính
        [Required, MaxLength(150)]
        public string? Name { get; set; }
        [Required, MaxLength(3500)]
        public string? Description { get; set; }

        [Required, Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18, 3)")]
        public decimal Price { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required, MaxLength(255)]
        public string? ImageUrl { get; set; }



        // Foreign Key
        [Required]
        public Guid BrandId { get; set; }
        public Brand? Brand { get; set; }

        // Foreign Key to Video
        public Guid? VideoId { get; set; }
        public Video? Video { get; set; }


        // 1 Product có nhiều ProductCategories
        public ICollection<ProductCategory>? ProductCategories { get; set; }

        // 1 Product có nhiều Reviews
        public ICollection<Review>? Reviews { get; set; }

        public ICollection<ProductImage>? ProductImages { get; set; }

        public ICollection<CartItem>? CartItems { get; set; }

    }
}
