using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface ITokenRepository
    {
        string CreateJWTToken(User user, List<string> Roles);
    }
}
