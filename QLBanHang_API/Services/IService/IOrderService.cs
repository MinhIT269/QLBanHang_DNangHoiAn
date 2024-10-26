using PBL6_QLBH.Models;
using QLBanHang_API.Dto;

namespace QLBanHang_API.Services.IService
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllOrders(string username);
        Task<List<OrderDetailDto>> GetOrderDetails(Guid id );
        Task<OrderDto> UpdateOrder(Guid id, string status);
        
    }
}
