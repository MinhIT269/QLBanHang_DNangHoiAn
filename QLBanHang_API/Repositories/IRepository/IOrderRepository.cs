using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task SaveChangesAsync();
        Task<List<OrderDetail>> CreateOrderDetailsAsync(List<OrderDetail> orderDetails);
        Task<Order> GetOrderByIdAsync(Guid orderId);
        IQueryable<Order> GetOrdersByStatus(string status);
        Task<List<Order>> GetAllOrderAsync(Guid? id, string searchQuery);
        Task<Order> GetOrderDetailAsync(Guid? id);
        Task<Order> UpdateOrderAsync(Guid id, string status);
        IQueryable<Order> GetFilteredOrders(string searchQuery, string sortCriteria, bool isDescending);
        Task<int> TotalOrders();
        Task<int> TotalOrdersSuccess();
        Task<int> TotalOrdersPending();
        Task<int> TotalOrdersCancel();
        Task<int> TotalOrdersByUser(Guid userId);
        Task<int> TotalOrdersSuccessByUser(Guid userId);
        Task<int> TotalOrdersPendingByUser(Guid userId);
        Task<decimal> GetTotalAmountOfCompletedOrdersAsync();
        Task<decimal> SumCompletedOrdersAmountByUser(Guid userId);
        Task UpdateProductAfterSucessAsync(Order order);

        Task<Object> MissionForBeginnerStatus(Guid userId);
        Task<Order> CreateOrderAsync(Order order);
        Task<Dictionary<string, decimal>> GetOrderStatistics(string period);
	}
}
