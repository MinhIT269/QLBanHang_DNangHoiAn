
using PBL6.Dto;
using PBL6.Dto.Request;
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
        Task<OrderDetailDto> GetOrderDetails(Guid id);
		Task<OrderDto> UpdateOrder(Guid id, string status);
        Task<List<OrderDto>> GetFilteredOrders(int page, int pageSize, string seachQuery, string sortCriteria, bool isDescending);
        Task<int> GetTotalOrdersAsync(string searchQuery);
        Task<int> TotalOrders();
        Task<int> TotalOrdersSuccess();
        Task<int> TotalOrdersPending();
        Task<decimal> GetTotalAmountOfCompletedOrdersAsync();
        Task<int> TotalOrdersCancel();
        Task<Object> CheckMissionForBeginner(Guid userId);
        Task<List<OrderDto>> GetAllOrders(Guid id, string searchQuery, int page = 1, int pageSize = 5);
        Task<OrderDto> CreateOrder(OrderRequest orderRequest);
        Task<List<OrderDetailDto>> CreateOrderDetail(List<OrderDetailsRequest> orderDetail);
        Task<int> TotalOrdersByUser(Guid userId);
        Task<int> TotalOrdersSuccessByUser(Guid userId);
        Task<int> TotalOrdersPendingByUser(Guid userId);
        Task<decimal> SumCompletedOrdersAmountByUser(Guid userId);
        Task<Dictionary<string, decimal>> GetOrderStatistics(string period);

	}
}
