namespace PBL6.Dto.Request
{
    public class CartItemRequest
    {
        public Guid CartItemId { get; set; }

        public int Quantity { get; set; }

        public Guid CartId { get; set; } 

        public Guid ProductId { get; set; } 
    }
}
