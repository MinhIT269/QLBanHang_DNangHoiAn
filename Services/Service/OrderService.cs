using Microsoft.EntityFrameworkCore;
using PBL6.Repositories.IRepository;
using PBL6_BackEnd.Services.Service;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6_BackEnd.Services.ServiceImpl
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;

        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IVnPayService _vnPayService;
        public OrderService(
         IOrderRepository orderRepository,
         IOrderDetailRepository orderDetailRepository,
         IVnPayService vnPayService)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _vnPayService = vnPayService;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _orderRepository.AddOrderAsync(order);
        }

        public async Task<Order> AddOrderWithDetailsAsync(Order newOrder,string promoteId)
        {


            newOrder.OrderId = Guid.NewGuid();
            newOrder.OrderDate = DateTime.Now;
            newOrder.TotalAmount = await _vnPayService.CalculateTotalPriceOfAOrder(newOrder,promoteId);
            newOrder.UserId = Guid.Parse("d4e56743-ff2c-41d3-957d-576e9f574c5d");
            newOrder.PromotionId = Guid.Parse("78913b9e-9d5a-40c4-89ad-813c72d4b1f7");

            await _orderRepository.AddOrderAsync(newOrder);

            foreach (var detail in newOrder.OrderDetails)
            {
                await _orderDetailRepository.AddOrderDetailAsync(detail);
            }

            await _orderRepository.SaveChangesAsync();

            return newOrder;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public IQueryable<Order> getOrderByStatus(string status)
        {
            return _orderRepository.GetOrdersByStatus(status); 
        }

        public async Task SaveChangeAsync()
        {
            await _orderRepository.SaveChangesAsync();
        }
    }
}
