using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface IOrderDetailRepository
    {
        Task AddOrderDetailAsync(OrderDetail detail);
    }
}
