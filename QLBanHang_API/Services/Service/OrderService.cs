using AutoMapper;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Dto.Request;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Services.IService;
using System.Reflection.Metadata.Ecma335;

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
        public async Task<OrderDto> CreateOrder(OrderRequest orderRequest)
        {
            var orderAdd = new Order()
            {
                OrderId = orderRequest.OrderId,
                UserId = orderRequest.UserId,
                OrderDate = orderRequest.OrderDate,
                TotalAmount = orderRequest.TotalAmount,
                Status = orderRequest.Status,
                PromotionId = orderRequest.PromotionId
            };
            var orderDomain = await orderRepository.CreateOrderAsync(orderAdd);
            var orderDto = mapper.Map<OrderDto>(orderDomain);
            return orderDto;
        }
        public async Task<List<OrderDetailDto>> CreateOrderDetail(List<OrderDetailsRequest> orderDetailsRequests)
        {
            var orderDetailsAdd = mapper.Map<List<OrderDetail>>(orderDetailsRequests);
            var orderDetailDomain = await orderRepository.CreateOrderDetailsAsync(orderDetailsAdd);
            var orderDetailDto = mapper.Map<List<OrderDetailDto>>(orderDetailDomain);
            return orderDetailDto;
        }
        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await orderRepository.GetOrderByIdAsync(orderId);
        }
        public async Task SaveChangeAsync()
        {
            await orderRepository.SaveChangesAsync();
        }
        public async Task UpdateOrderAfterCompleteTransaction(Order order)
        {
            if (order.Promotion != null)
            {
                Console.WriteLine("check !null gate");
                order.Promotion.MaxUsage--;
                await orderRepository.SaveChangesAsync();
            }
            foreach (var detail in order.OrderDetails)
            {
                detail.Product.Stock--;
                await orderRepository.SaveChangesAsync();
            }
        }
    }
}
