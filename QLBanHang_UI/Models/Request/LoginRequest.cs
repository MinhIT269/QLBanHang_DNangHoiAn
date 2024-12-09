using System.ComponentModel.DataAnnotations;

namespace QLBanHang_UI.Models.Request
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
