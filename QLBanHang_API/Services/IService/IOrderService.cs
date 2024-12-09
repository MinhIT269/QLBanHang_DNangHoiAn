using PBL6_QLBH.Models;
using QLBanHang_API.Dto;

namespace QLBanHang_API.Services.IService
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllOrders(Guid id, string searchQuery, int page = 1, int pageSize = 5);
        Task<OrderDetailDto> GetOrderDetails(Guid id );
        Task<OrderDto> UpdateOrder(Guid id, string status);
        Task<List<OrderDto>> GetFilteredOrders(int page, int pageSize, string seachQuery, string sortCriteria, bool isDescending);
        Task<int> GetTotalOrdersAsync(string searchQuery);
		Task<int> TotalOrders();
		Task<int> TotalOrdersSuccess();
		Task<int> TotalOrdersPending();
		Task<int> TotalOrdersCancel();
        Task<int> TotalOrdersByUser(Guid userId);
        Task<int> TotalOrdersSuccessByUser(Guid userId);
        Task<int> TotalOrdersPendingByUser(Guid userId);
        Task<decimal> SumCompletedOrdersAmountByUser(Guid userId);


    }
}
