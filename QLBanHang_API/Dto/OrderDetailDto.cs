using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using QLBanHang_API.Dto;

namespace PBL6.Dto
{
	public class OrderDetailDto
	{
		public UserInfoDto UserInfo { get; set; }
		public Guid OrderId { get; set; }        // Foreign Key
		public string? Status { get; set; }
		public DateTime? OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal? DiscountPercentage { get; set; }

		//public Order? Order { get; set; }     // Navigation property
		public List<ProductDetailOrder>? Products { get; set; } // Navigation property
	}
}
