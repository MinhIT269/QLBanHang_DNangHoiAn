using QLBanHang_API.Dto;

namespace QLBanHang_API.Services.IService
{
    public interface IPromotionService
    {
        Task<List<PromotionDto>> GetAllPromotion();
        Task<PromotionDto> GetPromotion(Guid code);
        Task<PromotionDto> UpdatePromotion(UpPromotionDto promotion);
        Task<PromotionDto> DeletePromotion(Guid id);
        Task<PromotionDto> AddPromotion(AddPromotionDto promotion);
        Task<List<PromotionDto>> GetFilteredPromotionsQuery(int page, int pageSize, string searchQuery, string sortCriteria, bool isDescending);
        Task<int> GetTotalPromotionAsync(string searchQuery);
    }
}
