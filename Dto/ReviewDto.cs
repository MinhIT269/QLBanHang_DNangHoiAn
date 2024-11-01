using PBL6_QLBH.Models;
using System.ComponentModel.DataAnnotations;

namespace PBL6.Dto
{
    public class ReviewDto
    {
        [Key]
        public Guid ReviewId { get; set; }    // Primary Key

        [Required]
        public Guid ProductId { get; set; }   // Foreign Key

        [Required]
        public Guid UserId { get; set; }      // Foreign Key

        [Required, Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        public ProductDto? Product { get; set; }

        public UserDto? User { get; set; }        // Navigation property
    }
}
