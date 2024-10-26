using PBL6_QLBH.Models;

namespace QLBanHang_API.Dto
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Guid RoleId { get; set; }
        public Role? role { get; set; }
    }
}
