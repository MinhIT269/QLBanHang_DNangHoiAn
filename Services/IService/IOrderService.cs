
using PBL6_QLBH.Models;

namespace PBL6_BackEnd.Services.Service
{
    public interface IOrderService
    {
        public IQueryable<Order> getOrderByStatus(string status);
        public Task AddOrderAsync(Order order);
        public Task SaveChangeAsync();

        public Task<Order> AddOrderWithDetailsAsync(Order newOrder);

        Task<Order> GetOrderByIdAsync(Guid orderId);
    }
}
