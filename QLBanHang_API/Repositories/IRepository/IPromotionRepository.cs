﻿using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface IPromotionRepository
    {
        Task<List<Promotion>> GetAllPromotionAsync();
        Task<Promotion> GetPromotionByCodeAsync(string? code);
        Task<Promotion> UpdatePromotionByIdAsync(Guid id, Promotion promotionUpdate);
        Task<Promotion> DeletePromotionByIdAsync(Guid id);
        Task<Promotion> AddPromotionAsync(Promotion promotion);
    }
}
