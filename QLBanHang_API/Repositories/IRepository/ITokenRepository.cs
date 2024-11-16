using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface ITokenRepository
    {
        string CreateJWTToken(User user, List<string> Roles );
    }
}
