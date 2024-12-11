using System.ComponentModel.DataAnnotations;

namespace QLBanHang_UI.Models.Request
{
    public class UpPromotionRequest
    {
        public Guid? PromotionId { get; set; }

        [Required, MaxLength(50)]
        public string? Code { get; set; }

        public decimal Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int MaxUsage { get; set; }
    }
}
