namespace QLBanHang_API.Dto.Response
{
    public class LoginResponse
    {
        public string JwtToken { get; set; }
        public Guid UserId { get; set; }
    }
}
