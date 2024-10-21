using PBL6_QLBH.Models;
using System.ComponentModel.DataAnnotations;

namespace QLBanHang_API.Request
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
