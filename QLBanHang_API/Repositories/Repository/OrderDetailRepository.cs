using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using System;

namespace PBL6.Repositories.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly DataContext _context;

        public OrderDetailRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddOrderDetailAsync(OrderDetail detail)
        {
            await _context.OrderDetails.AddAsync(detail);
        }
    }
}
