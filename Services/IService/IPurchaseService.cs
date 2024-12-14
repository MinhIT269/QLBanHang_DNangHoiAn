using PBL6.Dto;

namespace PBL6.Services.IService
{
    public interface IPurchaseService
    {
        Task<List<OrderDto>> GetFilteredOrders(int page, int pageSize, string seachQuery, string sortCriteria);
        Task<int> GetTotalOrdersAsync(string searchQuery, string sortCriteria);
        Task<object> GetPaymentMethodStatisticsAsync();
    }
}
