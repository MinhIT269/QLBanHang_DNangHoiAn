using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
	public class Brand
	{
		[Key]
		public Guid BrandId { get; set; } // Primary Key

		[Required, MaxLength(120)]
		public string? BrandName { get; set; }

		[MaxLength(900)]
		public string? Description { get; set; }

		// Navigation property
		public ICollection<Product>? Products { get; set; }
		// - Một thương hiệu có nhiều địa điểm
		public ICollection<Location>? Locations { get; set; }
	}
}
