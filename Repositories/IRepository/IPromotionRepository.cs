using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface IPromotionRepository
    {
        Task<Promotion?> getPromotionByPromotionCode(string code);
        Task<List<Promotion>> GetAllPromotionAsync();
        Task<Promotion> GetPromotionByCodeAsync(Guid code);
        Task<Promotion> UpdatePromotionByIdAsync(Guid id, Promotion promotionUpdate);
        Task<Promotion> DeletePromotionByIdAsync(Guid id);
        Task<Promotion> AddPromotionAsync(Promotion promotion);

        IQueryable<Promotion> GetFilteredPromotionsQuery(string searchQuery, string sortCriteria, bool isDescending);
    }
}
