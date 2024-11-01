using PBL6.Services.Service;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Services.ServiceImpl
{
    public class PromotionService : IPromotionService
    {
        private readonly DataContext _context;
        public PromotionService(DataContext context)
        {
            _context = context;
        }

        public IQueryable<object> getAllPromotion()
        {
            return _context.Promotions.Select(p => new
            {
                p.PromotionId,
                p.Code,
                p.Percentage,
                p.StartDate,
                p.EndDate,
                p.MaxUsage,
                Content = $"Mã {p.Code} giảm {p.Percentage}%"
            }).AsQueryable();
        }
    }
}
