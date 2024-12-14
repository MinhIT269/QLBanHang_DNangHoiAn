using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class Cart
    {
        [Key]
        public Guid CartId { get; set; }

        [Required]
        // Foreign Key to User
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public User? User { get; set; }

        // 1 Cart có nhiều CartItems
        public ICollection<CartItem>? CartItems { get; set; }
    }
}
