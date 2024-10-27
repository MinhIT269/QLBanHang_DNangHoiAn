using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class ProductCategory
    {
        // [Required]
        [Key]
        public Guid ProductCategoryId { get; set; } // Thêm khóa chính
        public Guid ProductId { get; set; }    // Foreign Key
        public Guid CategoryId { get; set; }   // Foreign Key

        public Product? Product { get; set; }
        public Category? Category { get; set; }
    }
}
