using System.ComponentModel.DataAnnotations;

namespace QLBanHang_UI.Models
{
	public class BrandDto
	{
		[Key]
		public Guid BrandId { get; set; } // Primary Key

		[Required, MaxLength(120)]
		public string? BrandName { get; set; }

		[MaxLength(900)]
		public string? Description { get; set; }
	}
}
