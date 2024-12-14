namespace PBL6.Dto
{
    public class BrandDto
    {
        public Guid BrandId { get; set; } 
        public string? BrandName { get; set; }
        public ICollection<LocationDto>? Locations { get; set; }

    }
}
