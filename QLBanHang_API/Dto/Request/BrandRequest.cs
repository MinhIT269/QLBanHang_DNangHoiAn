using PBL6_QLBH.Models;
using System.ComponentModel.DataAnnotations;

namespace QLBanHang_API.Dto.Request
{
	public class BrandRequest
	{
		public Guid BrandId { get; set; } // Primary Key

		[Required, MaxLength(120)]
		public string? BrandName { get; set; }

		[MaxLength(900)]
		public string? Description { get; set; }
		public ICollection<LocationRequest> Locations { get; set; }
	}
}
