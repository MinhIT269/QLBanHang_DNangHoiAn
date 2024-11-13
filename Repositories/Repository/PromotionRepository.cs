using Microsoft.EntityFrameworkCore;
using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Repositories.Repository
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly DataContext _dataContext;


        public PromotionRepository(DataContext dataContext) {
            _dataContext = dataContext; 
        }

            public async Task<Promotion?> getPromotionByPromotionCode(string code)
            {
                return await _dataContext.Promotions.FirstOrDefaultAsync(p => p.Code == code) ;
            }
    }
}
