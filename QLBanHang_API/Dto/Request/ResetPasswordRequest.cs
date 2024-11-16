using System.ComponentModel.DataAnnotations;

namespace QLBanHang_API.Dto.Request
{
    public class ResetPasswordRequest
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
