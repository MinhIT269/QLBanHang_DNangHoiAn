using PBL6_BackEnd.Services.Service;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6_BackEnd.Services.ServiceImpl
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly DataContext _context;
        public OrderDetailService(DataContext context) {
            _context = context;
        }
        public async Task AddOrderDetaikAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
        }
    }
}
