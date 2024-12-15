using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PBL6.Dto.Request
{
    public class OrderDetailsRequest
    {
        public Guid OrderDetailId { get; set; }  // Primary Key
        public Guid OrderId { get; set; }        // Foreign Key
        public Guid ProductId { get; set; }      // Foreign Key

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required, Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18, 3)")]
        public decimal UnitPrice { get; set; }
    }
}
