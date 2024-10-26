using System.ComponentModel.DataAnnotations;

namespace QLBanHang_UI.Models
{
	public class ProductImageDto
	{
		[Key]
		public Guid ProductImageId { get; set; }
		public Guid ProductId { get; set; }

		[Required, MaxLength(255)]
		public string? ImageUrl { get; set; } // Đường dẫn đến ảnh chi tiết
	}
}
