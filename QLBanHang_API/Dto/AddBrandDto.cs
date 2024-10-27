using System.ComponentModel.DataAnnotations;

namespace QLBanHang_API.Dto
{
    public class AddBrandDto
    {
        [Required, MaxLength(120)]

        public string? BrandName { get; set; }
        [MaxLength(900)]
        public string? Description { get; set; }
    }
}
