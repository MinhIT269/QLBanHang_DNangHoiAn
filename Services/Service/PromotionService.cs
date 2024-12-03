using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PBL6.Dto;
using PBL6.Repositories.IRepository;
using PBL6.Services.Service;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using System.Reflection.Metadata.Ecma335;

namespace PBL6.Services.ServiceImpl
{
    public class PromotionService : IPromotionService
    {
        private readonly DataContext _context;
        private readonly IPromotionRepository promotionRepository;
        private readonly IMapper mapper;

        public PromotionService(DataContext context , IPromotionRepository promotionRepository, IMapper mapper)
        {
            _context = context;
            this.promotionRepository = promotionRepository;
            this.mapper = mapper;
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

        public async Task<PromotionDto> getPromotionByPromotionCode(string promotionCode)
        {
            Console.WriteLine("code:" + promotionCode);

            var promotion = await promotionRepository.getPromotionByPromotionCode(promotionCode);
            return mapper.Map<PromotionDto>(promotion);
        }

        public async Task<List<PromotionDto>> GetAllPromotion()
        {
            var promotions = await promotionRepository.GetAllPromotionAsync();
            var promotionsDto = mapper.Map<List<PromotionDto>>(promotions);
            return promotionsDto;
        }
        public async Task<PromotionDto> GetPromotion(Guid code)
        {
            var promotion = await promotionRepository.GetPromotionByCodeAsync(code);
            var promotionDto = mapper.Map<PromotionDto>(promotion);
            return promotionDto;
        }

        public async Task<PromotionDto> UpdatePromotion(UpPromotionDto promotionUpdate)
        {
            var promotion = mapper.Map<Promotion>(promotionUpdate);
            var promotionDomain = await promotionRepository.UpdatePromotionByIdAsync(promotion.PromotionId, promotion);
            var promotionDto = mapper.Map<PromotionDto>(promotionDomain);
            return promotionDto;
        }
        public async Task<PromotionDto> AddPromotion(AddPromotionDto promotionAdd)
        {
            var promotion = mapper.Map<Promotion>(promotionAdd);
            var promotions = await GetAllPromotion();
            foreach (var i in promotions)
            {
                if (i.Code == promotionAdd.Code)
                {
                    return null;
                }
            }
            var promotionDomain = await promotionRepository.AddPromotionAsync(promotion);
            var promotionDto = mapper.Map<PromotionDto>(promotionDomain);
            return promotionDto;
        }

        public async Task<PromotionDto> DeletePromotion(Guid id)
        {
            try
            {
                var promotion = await promotionRepository.DeletePromotionByIdAsync(id);
                var promotionDto = mapper?.Map<PromotionDto>(promotion);
                return promotionDto;
            }
            catch (InvalidOperationException ex)
            {
                // Log lỗi nếu cần thiết
                throw new Exception($"Xóa Promotion thất bại: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                throw new Exception("Đã xảy ra lỗi khi xóa Promotion.");
            }
        }

        public async Task<List<PromotionDto>> GetFilteredPromotionsQuery(int page, int pageSize, string searchQuery, string sortCriteria, bool isDescending)
        {
            var query = promotionRepository.GetFilteredPromotionsQuery(searchQuery, sortCriteria, isDescending);

            var pagedPromotions = await query.Skip((page - 1) * pageSize)
                                             .Take(pageSize)
                                             .ProjectTo<PromotionDto>(mapper.ConfigurationProvider)
                                             .ToListAsync();
            return pagedPromotions;
        }
        public async Task<int> GetTotalPromotionAsync(string searchQuery)
        {
            var query = promotionRepository.GetFilteredPromotionsQuery(searchQuery, "name", false);
            return await query.CountAsync();
        }
    }
}
