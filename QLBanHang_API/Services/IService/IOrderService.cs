using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Dto.Request;

namespace QLBanHang_API.Services.IService
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllOrders(string username);
        Task<List<OrderDetailDto>> GetOrderDetails(Guid id );
        Task<OrderDto> UpdateOrder(Guid id, string status);
        Task<OrderDto> CreateOrder(OrderRequest orderRequest);
        Task<List<OrderDetailDto>> CreateOrderDetail(List<OrderDetailsRequest> orderDetail);
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task SaveChangeAsync();
        Task UpdateOrderAfterCompleteTransaction(Order order);
    }
}
