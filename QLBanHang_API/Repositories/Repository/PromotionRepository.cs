using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using Microsoft.EntityFrameworkCore;
using QLBanHang_API.Repositories.IRepository;
namespace QLBanHang_API.Repositories.Repository
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly DataContext dbContext;
        public PromotionRepository(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }


        //Get all Promotions 
        public async Task<List<Promotion>> GetAllPromotionAsync()
        {
            var promotions = await dbContext.Promotions.ToListAsync();
            return promotions;
        }

        // Get Promotion By Code
        public async Task<Promotion> GetPromotionByCodeAsync(Guid code)
        {
            var promotion = await dbContext.Promotions
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync(x => x.PromotionId == code);

            // Trả về promotion hoặc null nếu không tìm thấy
            return promotion;
        }

        public async Task<Promotion> GetPromotionAsync(string code)
        {
            if(code == null)
            {
                return null;
            }
            var promotion = await dbContext.Promotions.FirstOrDefaultAsync(p=>p.Code == code);
            return promotion;
        }
        //Update Promotion by id 
        public async Task<Promotion> UpdatePromotionByIdAsync(Guid id, Promotion promotionUpdate)
        {
            var promotion = await dbContext.Promotions.FirstOrDefaultAsync(x => x.PromotionId == id);
            if (promotion == null)
            {
                return null;
            }

            promotion.Code = promotionUpdate.Code;
            promotion.Percentage = promotionUpdate.Percentage;
            promotion.StartDate = promotionUpdate.StartDate;
            promotion.EndDate = promotionUpdate.EndDate;
            promotion.MaxUsage = promotionUpdate.MaxUsage;
            await dbContext.SaveChangesAsync();
            return promotion;
        }

        //Delete Promotion by Id 

        public async Task<Promotion> DeletePromotionByIdAsync(Guid id)
        {
            var promotion = await dbContext.Promotions
                .Include(p => p.Orders) // Bao gồm thông tin liên kết với Orders
                .FirstOrDefaultAsync(p => p.PromotionId == id);

            if (promotion == null)
            {
                throw new KeyNotFoundException("Promotion không tồn tại.");
            }

            if (promotion.Orders != null && promotion.Orders.Any())
            {
                throw new InvalidOperationException("Không thể xóa Promotion vì đã được sử dụng trong Orders.");
            }

            dbContext.Promotions.Remove(promotion);
            await dbContext.SaveChangesAsync();
            return promotion;
        }

        //Add Promotion 
        public async Task<Promotion> AddPromotionAsync(Promotion promotion)
        {
            promotion.PromotionId = Guid.NewGuid();
            await dbContext.Promotions.AddAsync(promotion);
            await dbContext.SaveChangesAsync();
            return promotion;
        }

        public IQueryable<Promotion> GetFilteredPromotionsQuery(string searchQuery, string sortCriteria, bool isDescending)
        {
            var query = dbContext.Promotions.AsQueryable();

            if(!string.IsNullOrEmpty(searchQuery)){
                query = query.Where(c => EF.Functions.Collate(c.Code!, "SQL_Latin1_General_CP1_CI_AI").Contains(searchQuery));
            }

            query = sortCriteria switch
            {
                "name" => isDescending ? query.OrderByDescending(c => c.Code) : query.OrderBy(c => c.Code),
                "endDate" => isDescending ? query.OrderByDescending(c => c.EndDate) : query.OrderBy(c => c.EndDate),
                "startDate" => isDescending ? query.OrderBy(c => c.StartDate) : query.OrderByDescending(c => c.StartDate),
                _ => query
            };

            return query;
        }
        public async Task<object> GetPromotionStatsAsync()
        {
            // Tổng số Promotion
            var totalPromotions = await dbContext.Promotions.CountAsync();

            // Số chương trình khuyến mãi còn hiệu lực
            var activePromotions = await dbContext.Promotions
                                                   .Where(p => p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now)
                                                   .CountAsync();

            // Số chương trình khuyến mãi đã hết hạn
            var expiredPromotions = await dbContext.Promotions
                                                    .Where(p => p.EndDate < DateTime.Now)
                                                    .CountAsync();

            // Số chương trình khuyến mãi chưa bắt đầu
            var upcomingPromotions = await dbContext.Promotions
                                                     .Where(p => p.StartDate > DateTime.Now)
                                                     .CountAsync();

            return new
            {
                TotalPromotions = totalPromotions,
                ActivePromotions = activePromotions,
                ExpiredPromotions = expiredPromotions,
                UpcomingPromotions = upcomingPromotions
            };
        }
    }
}
