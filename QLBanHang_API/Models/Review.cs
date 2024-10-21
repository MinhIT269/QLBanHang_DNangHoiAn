using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class Review
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

        public Product? Product { get; set; }  // Navigation property
        public User? User { get; set; }        // Navigation property
    }
}
