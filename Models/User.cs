using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class User : IdentityUser<Guid>
    {
        [Key]
        public Guid UserId
        {
            get => Id;
            set => Id = value;
        }
        // 1 User có nhiều Reviews
        public ICollection<Review>? Reviews { get; set; }

        // 1 User có nhiều PaymentDetails
        public ICollection<PaymentDetail>? PaymentDetails { get; set; }

        // 1 User có nhiều Orders
        public ICollection<Order> Orders { get; set; }

        // 1-1 với UserInfo
        public ICollection<Cart> Carts { get; set; }

        public UserInfo? UserInfo { get; set; }
    }
}
