using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrderAsync(Guid? id, string searchQuery);
        Task<Order> GetOrderDetailAsync(Guid? id);
        Task<Order> UpdateOrderAsync(Guid id, string status);
        Task<Order> CreateOrderAsync(Order order);
        Task<List<OrderDetail>> CreateOrderDetailsAsync(List<OrderDetail> orderDetails);
        Task AddOrderAsync(Order order);
        Task SaveChangesAsync();
        Task<Order> GetOrderByIdAsync(Guid orderId);
        IQueryable<Order> GetFilteredOrders(string searchQuery, string sortCriteria, bool isDescending);
        Task<int> TotalOrders();
        Task<int> TotalOrdersSuccess();
        Task<int> TotalOrdersPending();
        Task<int> TotalOrdersCancel();
        Task<int> TotalOrdersByUser(Guid userId);
        Task<int> TotalOrdersSuccessByUser(Guid userId);
        Task<int> TotalOrdersPendingByUser(Guid userId);
        Task<decimal> SumCompletedOrdersAmountByUser(Guid userId);
        Task UpdateProductAfterSucessAsync(Order order);

    }
}
