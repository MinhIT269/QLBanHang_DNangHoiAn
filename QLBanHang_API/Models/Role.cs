using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class Role
    {
        [Key]
        public Guid RoleId { get; set; }

        [Required, MaxLength(10)]
        public string RoleName { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        // 1 Role có nhiều Users
        public ICollection<User> Users { get; set; }
    }
}
