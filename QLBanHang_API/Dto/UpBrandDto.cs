using System.ComponentModel.DataAnnotations;

namespace PBL6.Dto
{
    public class UpBrandDto
    {
        [Required, MaxLength(120)]
        public string? BrandName { get; set; }
        [MaxLength(900)]
        public string? Description { get; set; }
    }
}
