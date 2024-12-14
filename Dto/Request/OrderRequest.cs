using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PBL6.Dto.Request
{
    public class OrderRequest
    {
        public Guid OrderId { get; set; }    // Primary Key
        public DateTime? OrderDate { get; set; }

        [Required, Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18, 3)")]
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }

        public Guid UserId { get; set; }

        public Guid? PromotionId { get; set; }    // Foreign Key to Promotion
        //public decimal? DiscountPercentage { get; set; }
    }
}
