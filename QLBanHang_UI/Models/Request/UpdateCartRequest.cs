namespace QLBanHang_UI.Models.Request
{
	public class UpdateCartRequest
	{
		public Guid CartItemId { get; set; }
		public int Quantity { get; set; }
	}
}
