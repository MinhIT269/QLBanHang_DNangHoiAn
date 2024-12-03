using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PBL6.Dto;
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


    }
}
