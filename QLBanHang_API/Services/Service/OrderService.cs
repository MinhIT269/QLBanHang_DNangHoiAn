using AutoMapper;
using QLBanHang_API.Dto;
using QLBanHang_API.Repositories.IRepository;
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

        public async Task<List<OrderDetailDto>> GetOrderDetails(Guid id)
        {
            var orderDetailDomain = await orderRepository.GetOrderDetailAsync(id);
            if (orderDetailDomain == null)
            {
                return null;
            }

            var orderDetailDto = mapper.Map<List<OrderDetailDto>>(orderDetailDomain);
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
    }
}
