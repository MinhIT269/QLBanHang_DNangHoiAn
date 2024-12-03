using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task SaveChangesAsync();
        Task<Order> GetOrderByIdAsync(Guid orderId);
        IQueryable<Order> GetOrdersByStatus(string status);
        Task<List<Order>> GetAllOrderAsync(string? username);
        Task<Order> GetOrderDetailAsync(Guid? id);
        Task<Order> UpdateOrderAsync(Guid id, string status);
        IQueryable<Order> GetFilteredOrders(string searchQuery, string sortCriteria, bool isDescending);
        Task<int> TotalOrders();
        Task<int> TotalOrdersSuccess();
        Task<int> TotalOrdersPending();
        Task<int> TotalOrdersCancel();

    }
}
