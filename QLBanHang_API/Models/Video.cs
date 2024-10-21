using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class Video
    {
        [Key]
        public Guid VideoId { get; set; }

        [Required, MaxLength(150)]
        public string? VideoUrl { get; set; }
        public string? Description { get; set; }

        // 1 Video có thể có nhiều Products
        public ICollection<Product>? Products { get; set; }
    }
}
