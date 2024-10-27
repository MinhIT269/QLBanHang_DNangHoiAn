using PBL6_QLBH.Models;
using System.ComponentModel.DataAnnotations;

namespace PBL6.Dto
{
    public class CategoryDto
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
