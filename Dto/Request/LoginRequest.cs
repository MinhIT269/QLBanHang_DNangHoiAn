using System.ComponentModel.DataAnnotations;

namespace QLBanHang_API.Dto.Request
{
    public class LoginRequest
    {
        public string UserName {  get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
