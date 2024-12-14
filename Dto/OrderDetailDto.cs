using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PBL6.Dto
{
    public class OrderDetailDto
    {
        public Guid OrderDetailId { get; set; }  
        [Required]
        public Guid OrderId { get; set; }        // Foreign Key
        [Required]
        public Guid ProductId { get; set; }      // Foreign Key

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required, Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18, 3)")]
        public decimal UnitPrice { get; set; }

        //public Order? Order { get; set; }     // Navigation property
        public ProductDto? product { get; set; } // Navigation property
    }
}
