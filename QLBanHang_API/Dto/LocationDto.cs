using PBL6_QLBH.Models;

namespace QLBanHang_API.Dto
{
    public class LocationDto
    {
        public Guid LocationId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? YoutubeLink { get; set; }
        public Guid BrandId { get; set; }
        public BrandDto? Brand { get; set; }
    }
}
