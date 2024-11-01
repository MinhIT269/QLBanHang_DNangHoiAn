
using PBL6_QLBH.Models;

namespace PBL6_BackEnd.Services.Service
{
    public interface IOrderDetailService
    {
        public Task AddOrderDetaikAsync(OrderDetail orderDetail);
    }
}
