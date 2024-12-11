using System.ComponentModel.DataAnnotations;

namespace QLBanHang_UI.Models.Request
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
    }
}
