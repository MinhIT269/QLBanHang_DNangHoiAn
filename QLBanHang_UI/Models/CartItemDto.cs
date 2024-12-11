namespace QLBanHang_UI.Models
{
	public class CartItemDto
	{
        public Guid CartItemId { get; set; }

        public int Quantity { get; set; }

        public Guid CartId { get; set; } // Foreign Key to Cart

        public Guid ProductId { get; set; } // Foreign Key to Product
        public ProductDto? Product { get; set; }
    }
}
