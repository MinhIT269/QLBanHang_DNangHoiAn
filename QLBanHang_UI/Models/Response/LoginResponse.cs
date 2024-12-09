namespace QLBanHang_UI.Models.Response
{
    public class LoginResponse
    {
        public string JwtToken { get; set; }
        public Guid UserId { get; set; }
    }
}
