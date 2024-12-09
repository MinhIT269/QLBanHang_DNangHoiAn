using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PBL6_QLBH.Models
{
    public class Promotion
    {
        [Key]
        public Guid PromotionId { get; set; }

        [Required, MaxLength(50)]
        public string? Code { get; set; }

        [Required, Range(0, 100)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int MaxUsage { get; set; }

        // 1 Promotion có thể có nhiều Orders
        public ICollection<Order>? Orders { get; set; }

    }
}
