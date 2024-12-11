namespace QLBanHang_API.Dto.Request
{
    public class CartItemRequest
    {
        public Guid CartItemId { get; set; }

        public int Quantity { get; set; }

        public Guid CartId { get; set; } // Foreign Key to Cart

        public Guid ProductId { get; set; } // Foreign Key to Product
    }
}
