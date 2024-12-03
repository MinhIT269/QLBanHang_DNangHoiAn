using PBL6.Dto;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;

namespace PBL6.Services.Service
{
    public interface IPromotionService
    {
        public IQueryable<object> getAllPromotion();
        public Task<PromotionDto> getPromotionByPromotionCode(string promotionCode);

        Task<List<PromotionDto>> GetAllPromotion();
        Task<PromotionDto> GetPromotion(Guid code);
        Task<PromotionDto> UpdatePromotion(UpPromotionDto promotion);
        Task<PromotionDto> DeletePromotion(Guid id);
        Task<PromotionDto> AddPromotion(AddPromotionDto promotion);
        Task<List<PromotionDto>> GetFilteredPromotionsQuery(int page, int pageSize, string searchQuery, string sortCriteria, bool isDescending);
        Task<int> GetTotalPromotionAsync(string searchQuery);
    }
}
