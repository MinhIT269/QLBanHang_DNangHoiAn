namespace QLBanHang_UI.Models
{
    public class OrderDto
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
