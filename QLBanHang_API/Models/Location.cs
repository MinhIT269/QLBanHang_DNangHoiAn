using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class Location
    {
        [Key]
        public Guid LocationId { get; set; }  // Primary Key
        [Required, MaxLength(200)]
        public string? Name { get; set; }
        [MaxLength(400)]
        public string? Description { get; set; }
        public string? YoutubeLink { get; set; }

        public Guid BrandId { get; set; }
       // public Brand? Brand { get; set; }  // Navigation property
    }
}
