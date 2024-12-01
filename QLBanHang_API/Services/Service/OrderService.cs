using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QLBanHang_API.Dto;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Repositories.Repository;
using QLBanHang_API.Services.IService;

namespace QLBanHang_API.Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public OrderService (IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<List<OrderDto>> GetAllOrders(string username)
        {
            var ordersDomain = await orderRepository.GetAllOrderAsync(username);
            if (ordersDomain == null || !ordersDomain.Any())
            {
                return null;
            }
            var ordersDto = mapper.Map<List<OrderDto>>(ordersDomain);
            return ordersDto;
        }

        public async Task<OrderDetailDto> GetOrderDetails(Guid id)
        {
            var orderDetailDomain = await orderRepository.GetOrderDetailAsync(id);
            if (orderDetailDomain == null)
            {
                return null;
            }

            var orderDetailDto = mapper.Map<OrderDetailDto>(orderDetailDomain);
            return orderDetailDto;  
        }

        public async Task<OrderDto> UpdateOrder(Guid id, string status)
        {
            var orderDomain = await orderRepository.UpdateOrderAsync(id, status);
            if (orderDomain == null)
            {
                return null;
            }

            var orderDto = mapper.Map<OrderDto>(orderDomain);
            return orderDto;
        }
        public async Task<List<OrderDto>> GetFilteredOrders(int page, int pageSize, string searchQuery,string sortCriteria, bool isDescending)
        {
            var query = orderRepository.GetFilteredOrders(searchQuery, sortCriteria, isDescending);

            var pagedOrders = await query.Skip((page - 1) * pageSize)
                                             .Take(pageSize)
                                             .ProjectTo<OrderDto>(mapper.ConfigurationProvider)
                                             .ToListAsync();
            return pagedOrders;
        }

        public async Task<int> GetTotalOrdersAsync(string searchQuery)
        {
            var query = orderRepository.GetFilteredOrders(searchQuery, "name", false);
            return await query.CountAsync();
        }
		public async Task<int> TotalOrders()
        {
            return await orderRepository.TotalOrders();
        }
		public async Task<int> TotalOrdersSuccess() 
        {
            return await orderRepository.TotalOrdersSuccess();
        }
		public async Task<int> TotalOrdersPending() 
        {
            return await orderRepository.TotalOrdersPending();
        }
		public async Task<int> TotalOrdersCancel()
        {
            return await orderRepository.TotalOrdersCancel();
        }
	}
}
