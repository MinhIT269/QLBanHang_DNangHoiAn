using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PBL6_QLBH.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }    // Primary Key
        [Required]
        public Guid UserId { get; set; }     // Foreign Key
        public DateTime? OrderDate { get; set; }

        [Required, Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18, 3)")]
        public decimal TotalAmount { get; set; }

        [Required, MaxLength(50)]
        public string? Status { get; set; }

        public User? User { get; set; }   // Navigation property

        public ICollection<OrderDetail>? OrderDetails { get; set; }  // Navigation property for related OrderDetails

        //public decimal? DiscountPercentage { get; set; }
        public Guid? PromotionId { get; set; }    // Foreign Key to Promotion
        public Promotion? Promotion { get; set; }
        public Transaction? Transaction { get; set; }
    }
}
