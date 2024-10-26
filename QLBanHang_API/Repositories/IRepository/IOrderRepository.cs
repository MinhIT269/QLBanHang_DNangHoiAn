using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrderAsync(string? username);
        Task<List<OrderDetail>> GetOrderDetailAsync(Guid? id);
        Task<Order> UpdateOrderAsync(Guid id, string status);
    }
}
