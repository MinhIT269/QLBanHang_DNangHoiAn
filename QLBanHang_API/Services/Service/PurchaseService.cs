using AutoMapper;
using QLBanHang_API.Dto;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Services.IService;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QLBanHang_API.Repositories.Repository;
namespace QLBanHang_API.Services.Service
{
    public class PurchaseService : IPurchaseService
    {

        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMapper mapper;
        public PurchaseService(IPurchaseRepository purchaseRepository, IMapper mapper)
        {
            _purchaseRepository = purchaseRepository;
            this.mapper = mapper;
        }
        public async Task<List<OrderDto>> GetFilteredOrders(int page, int pageSize, string searchQuery, string sortCriteria)
        {
            var query = _purchaseRepository.GetFilteredOrdersPurchase(searchQuery, sortCriteria);

            var pagedOrders = await query.Skip((page - 1) * pageSize)
                                             .Take(pageSize)
                                             .ProjectTo<OrderDto>(mapper.ConfigurationProvider)
                                             .ToListAsync();
            return pagedOrders;
        }

        public async Task<int> GetTotalOrdersAsync(string searchQuery, string sortCriteria)
        {
            var query = _purchaseRepository.GetFilteredOrdersPurchase(searchQuery, sortCriteria);
            return await query.CountAsync();
        }
        public async Task<object> GetPaymentMethodStatisticsAsync()
        {
            return await _purchaseRepository.GetPaymentMethodStatisticsAsync();
        }
    }
}
