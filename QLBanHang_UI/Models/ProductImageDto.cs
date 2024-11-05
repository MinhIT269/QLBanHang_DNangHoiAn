using System.ComponentModel.DataAnnotations;

namespace QLBanHang_UI.Models
{
	public class ProductImageDto
	{
		public Guid ProductImageId { get; set; }
		public Guid ProductId { get; set; }
		public string? ImageUrl { get; set; } // Đường dẫn đến ảnh chi tiết
	}
}
