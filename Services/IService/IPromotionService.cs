using PBL6_QLBH.Models;

namespace PBL6.Services.Service
{
    public interface IPromotionService
    {
        public IQueryable<object> getAllPromotion();
    }
}
