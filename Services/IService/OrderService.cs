using Microsoft.EntityFrameworkCore;
using PBL6_BackEnd.Services.Service;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6_BackEnd.Services.ServiceImpl
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        public OrderService(DataContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public IQueryable<Order> getOrderByStatus(string status)
        {
        #pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            return _context.Orders
           .Include(o => o.OrderDetails) 
           .ThenInclude(od => od.Product) 
           .Where(o => o.Status == status); 
        #pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            ;
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
