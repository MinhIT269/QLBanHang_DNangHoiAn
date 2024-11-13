using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface IPromotionRepository
    {
        Task<Promotion?> getPromotionByPromotionCode(string code);
    }
}
