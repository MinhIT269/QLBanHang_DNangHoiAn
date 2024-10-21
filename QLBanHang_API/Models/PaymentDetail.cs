using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class PaymentDetail
    {
        [Key]
        public Guid PaymentDetailId { get; set; }
        public string? CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? CardHolderName { get; set; }

        [Required]
        public Guid UserId { get; set; }     // Foreign Key to User
        public User? User { get; set; }

        [Required]
        public Guid PaymentMethodId { get; set; }   // Foreign Key to PaymentMethod
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
