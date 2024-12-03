
using PBL6.Dto;
using PBL6_QLBH.Models;

namespace PBL6_BackEnd.Services.Service
{
    public interface IOrderService
    {
        public IQueryable<OrderDto> getOrderByStatus(string status);
        public Task AddOrderAsync(Order order);
        public Task SaveChangeAsync();

        public Task<Order> AddOrderWithDetailsAsync(Order newOrder,string promoteId);

        Task<Order> GetOrderByIdAsync(Guid orderId);

        public Task UpdateOrderAfterCompleteTransaction(Order order);


        Task<List<OrderDto>> GetAllOrders(string username);
        Task<OrderDetailDto> GetOrderDetails(Guid id);
        Task<OrderDto> UpdateOrder(Guid id, string status);
        Task<List<OrderDto>> GetFilteredOrders(int page, int pageSize, string seachQuery, string sortCriteria, bool isDescending);
        Task<int> GetTotalOrdersAsync(string searchQuery);
        Task<int> TotalOrders();
        Task<int> TotalOrdersSuccess();
        Task<int> TotalOrdersPending();
        Task<int> TotalOrdersCancel();
    }
}
