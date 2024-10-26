using System.ComponentModel.DataAnnotations;

namespace QLBanHang_UI.Models
{
	public class CategoryDto
	{
		[Key]
		public Guid CategoryId { get; set; }  // Primary Key

		[Required, MaxLength(100)]
		public string? CategoryName { get; set; }

		[MaxLength(200)]
		public string? Description { get; set; }
	}
}
