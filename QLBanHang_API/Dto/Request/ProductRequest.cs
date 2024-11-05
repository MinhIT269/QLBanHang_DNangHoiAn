using System.ComponentModel.DataAnnotations;

namespace QLBanHang_API.Dto.Request
{
	public class ProductRequest
	{
		[Key]
		public Guid ProductId { get; set; }  // Khóa chính
		[Required, MaxLength(150)]
		public string? Name { get; set; }

		[Required, MaxLength(250)]
		public string? MetaTitle { get; set; }

		[Required, MaxLength(3500)]
		public string? Description { get; set; }

		[Required, MaxLength(1000)]
		public string? MetaDescription { get; set; }

		[Required, Range(0, double.MaxValue)]
		public decimal Price { get; set; }


		[Required, Range(0, double.MaxValue)]
		public decimal? PromotionPrice { get; set; }

		[Required, Range(0, int.MaxValue)]
		public int Stock { get; set; }

		public DateTime CreatedDate { get; set; }

		public string? Warranty { get; set; }
		// Foreign Key
		public Guid BrandId { get; set; }

		// Foreign Key to Video
		public Guid? VideoId { get; set; }
		public List<Guid>? CategoryIds { get; set; }

		// Danh sách URL ảnh phụ cho ProductImages
		public List<string>? AdditionalImageUrls { get; set; }
	}

}
