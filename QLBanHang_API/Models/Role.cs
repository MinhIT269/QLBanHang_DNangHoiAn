using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PBL6_QLBH.Models
{
    public class Role :IdentityRole<Guid>
    {
        [MaxLength(200)]
        public string Description { get; set; }

        // 1 Role có nhiều Users
        public ICollection<User> Users { get; set; }
    }
}
