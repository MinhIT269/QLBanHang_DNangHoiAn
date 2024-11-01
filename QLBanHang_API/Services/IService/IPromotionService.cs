﻿using QLBanHang_API.Dto;

namespace QLBanHang_API.Services.IService
{
    public interface IPromotionService
    {
        Task<List<PromotionDto>> GetAllPromotion();
        Task<PromotionDto> GetPromotion(string code);
        Task<PromotionDto> UpdatePromotion(UpPromotionDto promotion);
        Task<PromotionDto> DeletePromotion(Guid id);
        Task<PromotionDto> AddPromotion(AddPromotionDto promotion);
    }
}
