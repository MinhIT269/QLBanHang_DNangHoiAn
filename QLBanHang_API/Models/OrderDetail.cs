using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PBL6_QLBH.Models
{
    public class OrderDetail
    {
        [Key]
        public Guid OrderDetailId { get; set; }  // Primary Key
        [Required]
        public Guid OrderId { get; set; }        // Foreign Key
        [Required]
        public Guid ProductId { get; set; }      // Foreign Key

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required, Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18, 3)")]
        public decimal UnitPrice { get; set; }

        public Order? Order { get; set; }     // Navigation property
        public Product? Product { get; set; } // Navigation property
    }
}
