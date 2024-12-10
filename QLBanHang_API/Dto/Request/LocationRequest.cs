using PBL6_QLBH.Models;
using System.ComponentModel.DataAnnotations;

namespace QLBanHang_API.Dto.Request
{
	public class LocationRequest
	{
		public Guid? LocationId { get; set; }
		[Required, MaxLength(200)]
		public string? Name { get; set; }
		[MaxLength(400)]
		public string? Description { get; set; }
		public string? YoutubeLink { get; set; }
		public Guid BrandId { get; set; }
	}
}
