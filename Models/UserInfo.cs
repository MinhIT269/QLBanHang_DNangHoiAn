using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class UserInfo
    {
        [Key]
        public Guid UserInfoId { get; set; }  // Primary Key

      //  [Required]
        public Guid UserId { get; set; }  // Foreign Key

        [Required, StringLength(50)]
        public string? Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        [Required, StringLength(30)]
        public string? FirstName { get; set; }

        [Required, StringLength(30)]
        public string? LastName { get; set; }

        // Gender mac dinh la false
        public bool Gender { get; set; }  // Gender: nếu cần nullable, hãy dùng bool?; nếu không, giữ nguyên bool

        // Navigation property
        public User? User { get; set; }
    }
}
