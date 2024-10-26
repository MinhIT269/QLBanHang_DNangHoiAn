using QLBanHang_API.Dto;


namespace PBL6_QLBH.DTOs
{
	public class CartDto
	{
		public Guid CartId { get; set; }

		public Guid UserId { get; set; }

		public DateTime CreatedDate { get; set; }

		public List<CartItemDto>? CartItems { get; set; }
	}
}