﻿namespace QLBanHang_UI.Models
{
    public class PromotionDto
    {
        public Guid PromotionId { get; set; }
        public string? Code { get; set; }
        public decimal Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxUsage { get; set; }
    }
}