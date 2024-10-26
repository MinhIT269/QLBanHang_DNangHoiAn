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
        public async Task<Promotion> GetPromotionByCodeAsync(string? code)
        {
            var promotion = await dbContext.Promotions
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync(x => x.Code == code);

            // Trả về promotion hoặc null nếu không tìm thấy
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
            var promotion = await dbContext.Promotions.FirstOrDefaultAsync(x => x.PromotionId == id);
            if (promotion == null)
            {
                return null;
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
    }
}
