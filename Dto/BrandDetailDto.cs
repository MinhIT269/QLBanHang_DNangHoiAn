namespace PBL6.Dto
{
    public class BrandDetailDto
    {
        public Guid BrandId { get; set; } // Primary Key
        public string? BrandName { get; set; }
        public int StockAvailable { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public ICollection<LocationDto>? Locations { get; set; }
    }
}
