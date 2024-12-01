using PBL6_QLBH.Models;

namespace QLBanHang_API.Dto
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }    // Primary Key
        public Guid UserId { get; set; }     // Foreign Key
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public DateTime? OrderDate { get; set; }

        public decimal TotalAmount { get; set; }
        public int totalProduct { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public Guid PromotionId { get; set; }    // Foreign Key to Promotion
        public string? Code { get; set; }
        public string? Status{ get; set; }
    }
}
