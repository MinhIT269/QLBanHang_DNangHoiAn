using System.ComponentModel.DataAnnotations;

namespace QLBanHang_UI.Models
{
	public class BrandDto
	{
		public Guid BrandId { get; set; } // Primary Key

		public string? BrandName { get; set; }
	}
}
