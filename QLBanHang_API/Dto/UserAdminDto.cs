namespace PBL6.Dto
{
    public class UserAdminDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public int Order { get; set; }
    }
}
