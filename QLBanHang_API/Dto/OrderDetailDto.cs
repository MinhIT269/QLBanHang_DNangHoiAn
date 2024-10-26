using PBL6_QLBH.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using QLBanHang_API.Dto;
using QLBanHang_API.Request;
namespace QLBanHang_API.Dto
{
    public class OrderDetailDto
    {
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

        //public Order? Order { get; set; }     // Navigation property
        public ProductDto? product { get; set; } // Navigation property
    }
}
