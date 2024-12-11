namespace QLBanHang_API.Dto.Request
{
    public class VnPaymentRequestModel
    {
        public Guid OrderId { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public double Ammount { get; set; }

        public DateTime CreatedDate { get; set; }
        //public string CallbackUrl { get; set; }
    }
}
