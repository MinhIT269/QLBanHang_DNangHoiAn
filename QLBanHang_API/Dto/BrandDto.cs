namespace QLBanHang_API.Dto
{
    public class BrandDto
    {
        public Guid BrandId { get; set; } // Primary Key

        public string? BrandName { get; set; }

        public string? Description { get; set; }

        // Navigation property
        //public ICollection<Product>? Products { get; set; }
        //  Một thương hiệu có nhiều địa điểm
        //public ICollection<Location>? Locations { get; set; }
    }
}
