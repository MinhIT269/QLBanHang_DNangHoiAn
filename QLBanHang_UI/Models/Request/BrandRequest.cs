using System.ComponentModel.DataAnnotations;

namespace QLBanHang_UI.Models.Request
{
	public class BrandRequest
	{
		public Guid BrandId { get; set; } // Primary Key

		[Required]
		[MaxLength(120)]
		public string? BrandName { get; set; }

		[MaxLength(600)]
		public string? Description { get; set; }
		public List<LocationRequest> Locations { get; set; } = new List<LocationRequest>();
	}
}
