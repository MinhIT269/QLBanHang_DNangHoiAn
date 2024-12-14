using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class Video
    {
        [Key]
        public Guid VideoId { get; set; }

        [Required, MaxLength(500)]
        public string? VideoUrl { get; set; }
        public string? Description { get; set; }

    }
}
