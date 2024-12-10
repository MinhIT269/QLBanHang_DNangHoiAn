namespace QLBanHang_UI.Models
{
    public class UserInfoDto
    {
        public Guid UserInfoId { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserDto? user { get; set; }
    }
}
