using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PBL6.Dto;
using PBL6.Dto.Request;
using PBL6.Repositories.IRepository;
using PBL6.Repositories.Repository;
using PBL6_BackEnd.Services.Service;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6_BackEnd.Services.ServiceImpl
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;

        private readonly IOrderRepository orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IVnPayService _vnPayService;
        private readonly IMapper mapper; 
       
        public OrderService(
         IOrderRepository orderRepository,
         IOrderDetailRepository orderDetailRepository,
         IVnPayService vnPayService,IMapper mapper)
        {
            this.orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _vnPayService = vnPayService;
            this.mapper = mapper;
        }

        public async Task AddOrderAsync(Order order)
        {
            await orderRepository.AddOrderAsync(order);
        }

        public async Task<Order> AddOrderWithDetailsAsync(Order newOrder,string promoteId)
        {


            newOrder.OrderId = Guid.NewGuid();
            newOrder.OrderDate = DateTime.Now;
            newOrder.TotalAmount = await _vnPayService.CalculateTotalPriceOfAOrder(newOrder,promoteId);
            newOrder.UserId = Guid.Parse("d4e56743-ff2c-41d3-957d-576e9f574c5d");
            newOrder.PromotionId = Guid.Parse(promoteId);

            await orderRepository.AddOrderAsync(newOrder);

            foreach (var detail in newOrder.OrderDetails)
            {
                await _orderDetailRepository.AddOrderDetailAsync(detail);
            }

            await orderRepository.SaveChangesAsync();

            return newOrder;
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

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await orderRepository.GetOrderByIdAsync(orderId);
        }

        public IQueryable<OrderDto> getOrderByStatus(string status)
        {
            var orders = orderRepository.GetOrdersByStatus(status); 

            var orderDtos = orders.Select(order => mapper.Map<OrderDto>(order));

            return orderDtos;
        }

        public async Task SaveChangeAsync()
        {
            await orderRepository.SaveChangesAsync();
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
        public async Task<List<OrderDto>> GetFilteredOrders(int page, int pageSize, string searchQuery, string sortCriteria, bool isDescending)
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

        public Task<object> CheckMissionForBeginner(Guid userId)
        {
            return orderRepository.MissionForBeginnerStatus(userId);
        }

        public async Task<List<OrderDto>> GetAllOrders(Guid id, string searchQuery, int page = 1, int pageSize = 5)
        {
            var ordersDomain = await orderRepository.GetAllOrderAsync(id, searchQuery);

            if (ordersDomain == null || !ordersDomain.Any())
            {
                return null;
            }

            // Áp dụng phân trang
            ordersDomain = ordersDomain
                .Skip((page - 1) * pageSize) // Bỏ qua số lượng phần tử của các trang trước đó
                .Take(pageSize) // Lấy số lượng phần tử giới hạn cho trang hiện tại
                .ToList();

            // Chuyển đổi sang DTO
            var ordersDto = mapper.Map<List<OrderDto>>(ordersDomain);
            return ordersDto;
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

        public async Task<int> TotalOrdersByUser(Guid userId)
        {
            return await orderRepository.TotalOrdersByUser(userId);
        }
        public async Task<int> TotalOrdersSuccessByUser(Guid userId)
        {
            return await orderRepository.TotalOrdersSuccessByUser(userId);
        }
        public async Task<int> TotalOrdersPendingByUser(Guid userId)
        {
            return await orderRepository.TotalOrdersPendingByUser(userId);
        }
        public async Task<decimal> SumCompletedOrdersAmountByUser(Guid userId)
        {
            return await orderRepository.SumCompletedOrdersAmountByUser(userId);
        }
    }
}
