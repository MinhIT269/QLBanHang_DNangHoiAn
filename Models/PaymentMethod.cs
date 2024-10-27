using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class PaymentMethod
    {
        [Key]
        public Guid PaymentMethodId { get; set; }

        [Required, MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(400)]
        public string? Description { get; set; }

        public ICollection<PaymentDetail>? PaymentDetails { get; set; }  // 1 PaymentMethod có thể có nhiều PaymentDetails

        public ICollection<Transaction>? Transactions { get; set; }  // Một PaymentMethod có thể có nhiều Transactions
    }
}
