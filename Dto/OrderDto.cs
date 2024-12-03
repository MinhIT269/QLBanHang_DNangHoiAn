using PBL6_QLBH.Models;

namespace PBL6.Dto
{
    public class OrderDto
    {

        public Guid OrderId { get; set; }    // Primary Key
        public Guid UserId { get; set; }     // Foreign Key
        public DateTime? OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Status { get; set; }

        public UserDto? User { get; set; }   // Navigation property
        public ICollection<OrderDetailDto>? OrderDetails { get; set; }  // Navigation property for related OrderDetails

        public Guid PromotionId { get; set; }    
        public PromotionDto? Promotion { get; set; }
  
    }
}
