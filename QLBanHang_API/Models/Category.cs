using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }  // Primary Key

        [Required, MaxLength(100)]
        public string? CategoryName { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        // Nhiều Categories có thể có nhiều Products (n-n)
        public ICollection<ProductCategory>? ProductCategories { get; set; }
    }
}
