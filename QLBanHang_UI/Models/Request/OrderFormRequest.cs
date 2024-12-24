using System.ComponentModel.DataAnnotations;

namespace QLBanHang_UI.Models.Request
{
    public class OrderFormRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string PromotionCode { get; set; }
    }
}
