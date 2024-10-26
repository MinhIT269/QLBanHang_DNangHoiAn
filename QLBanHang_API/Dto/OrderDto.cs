using PBL6_QLBH.Models;

namespace QLBanHang_API.Dto
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }    // Primary Key
        public Guid UserId { get; set; }     // Foreign Key
        public DateTime? OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Status { get; set; }


        //public ICollection<OrderDetail>? OrderDetails { get; set; }  // Navigation property for related OrderDetails

        public Guid PromotionId { get; set; }    // Foreign Key to Promotion
        public PromotionDto? Promotion { get; set; }
        public Transaction? Transaction { get; set; }
    }
}
