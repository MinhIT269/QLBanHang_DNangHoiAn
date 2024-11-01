using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task SaveChangesAsync();

        Task<Order> GetOrderByIdAsync(Guid orderId);


        IQueryable<Order> GetOrdersByStatus(string status);
    }
}
