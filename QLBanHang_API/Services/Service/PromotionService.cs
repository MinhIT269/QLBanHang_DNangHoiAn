using AutoMapper;
using Azure.Core;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Services.IService;

namespace QLBanHang_API.Services.Service
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository promotionRepository;
        private readonly IMapper mapper;
        public PromotionService(IPromotionRepository promotionRepository, IMapper mapper)
        {
            this.promotionRepository = promotionRepository;
            this.mapper = mapper;
        }

        public async Task<List<PromotionDto>> GetAllPromotion()
        {
            var promotions = await promotionRepository.GetAllPromotionAsync();
            var promotionsDto = mapper.Map<List<PromotionDto>>(promotions);
            return promotionsDto;
        }
        public async Task<PromotionDto> GetPromotion(string code)
        {
            var promotion = await promotionRepository.GetPromotionByCodeAsync(code);
            var promotionDto = mapper.Map<PromotionDto>(promotion);
            return promotionDto;
        }

        public async Task<PromotionDto> UpdatePromotion(PromotionDto promotionUpdate)
        {
            var promotion = mapper.Map<Promotion>(promotionUpdate);
            var promotionDomain = await promotionRepository.UpdatePromotionByIdAsync(promotion.PromotionId,promotion);
            var promotionDto = mapper.Map<PromotionDto>(promotionDomain);
            return promotionDto;
        }
        public async Task<PromotionDto> AddPromotion(PromotionDto promotionAdd)
        {
            var promotion = mapper.Map<Promotion>(promotionAdd);
            var promotionDomain = await promotionRepository.AddPromotionAsync(promotion);
            var promotionDto = mapper.Map<PromotionDto> (promotionDomain);
            return promotionDto;
        }

        public async Task<PromotionDto> DeletePromotion(Guid id)
        {
            var promotion = await promotionRepository.DeletePromotionByIdAsync(id);
            var promotionDto = mapper?.Map<PromotionDto>(promotion);
            return promotionDto;
        }

    }
}
