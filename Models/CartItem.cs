using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class CartItem
    {
        [Key]
        public Guid CartItemId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public Guid CartId { get; set; }  // Foreign Key to Cart
        public Cart? Cart { get; set; }

        [Required]
        public Guid ProductId { get; set; } // Foreign Key to Product
        public Product? Product { get; set; }
    }
}
