using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class Transaction
    {
        [Key]
        public Guid TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 3)")]  // Định dạng số tiền với 3 chữ số thập phân
        public decimal Amount { get; set; } 

        [Required]
        [StringLength(50)]  // Giới hạn độ dài của trường Status
        public string? Status { get; set; }

        [StringLength(500)]  // Giới hạn độ dài của TransactionDetails
        public string? TransactionDetails { get; set; }

        [Required]
        public Guid OrderId { get; set; }   // Foreign Key to Order
        public Order? Order { get; set; }
        [Required]
        public Guid PaymentMethodId { get; set; }  // Foreign Key to PaymentMethod
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
