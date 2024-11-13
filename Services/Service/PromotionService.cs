using AutoMapper;
using PBL6.Dto;
using PBL6.Repositories.IRepository;
using PBL6.Services.Service;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using System.Reflection.Metadata.Ecma335;

namespace PBL6.Services.ServiceImpl
{
    public class PromotionService : IPromotionService
    {
        private readonly DataContext _context;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IMapper _mapper;

        public PromotionService(DataContext context , IPromotionRepository promotionRepository, IMapper mapper)
        {
            _context = context;
            _promotionRepository = promotionRepository;
            _mapper = mapper;
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

            var promotion = await _promotionRepository.getPromotionByPromotionCode(promotionCode);
            return _mapper.Map<PromotionDto>(promotion);
        }
    }
}
