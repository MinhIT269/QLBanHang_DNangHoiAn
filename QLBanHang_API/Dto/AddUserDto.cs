using PBL6_QLBH.Models;
using System.ComponentModel.DataAnnotations;

namespace QLBanHang_API.Dto
{
    public class AddUserDto
    {
        public Guid UserId { get; set; } // Khóa chính

        [Required]
        [StringLength(25, MinimumLength = 5)]
        public string? Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Phải dài từ 7 đến 20 ký tự")]
        [DataType(DataType.Password)]
        public string? PasswordHash { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        public Guid RoleId { get; set; } // Khóa ngoại từ bảng Role
        public Role? Role { get; set; }

        // 1 User có nhiều Reviews
        public ICollection<Review>? Reviews { get; set; }

        // 1 User có nhiều PaymentDetails
        public ICollection<PaymentDetail>? PaymentDetails { get; set; }

        // 1 User có nhiều Orders
        // public ICollection<Order> Orders { get; set; }

        // 1-1 với UserInfo
        //public ICollection<Cart> Carts { get; set; }
    }
}
