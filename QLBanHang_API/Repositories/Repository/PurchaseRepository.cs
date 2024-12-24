using Microsoft.EntityFrameworkCore;
using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Repositories.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly DataContext dbContext;
        public PurchaseRepository(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IQueryable<Order> GetFilteredOrdersPurchase(string searchQuery, string sortCriteria)
        {
            var query = dbContext.Orders
                .Include(c => c.User)
                .Include(c => c.OrderDetails)!
                .ThenInclude(p => p.Product).AsQueryable();

            // Lọc theo trạng thái "Completed"
            query = query.Where(c => c.Status == "completed");

            // Lọc theo phương thức thanh toán hoặc lấy tất cả
            query = sortCriteria switch
            {
                "MoMo" => query.Where(c => c.Transaction!.PaymentMethod!.Name == "MoMo"),
                "VNPay" => query.Where(c => c.Transaction!.PaymentMethod!.Name == "VnPay"),
                "ZaloPay" => query.Where(c => c.Transaction!.PaymentMethod!.Name == "ZaloPay"),
                "All" or _ => query // Lấy tất cả các đơn hàng "Completed"
            };

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(order =>
                    order.OrderId.ToString().Substring(0, 8).Contains(searchQuery));
            }

            return query;
        }

        public async Task<object> GetPaymentMethodStatisticsAsync()
        {
            var stats = await dbContext.Orders
                .Where(order => order.Status == "completed" && order.Transaction != null && order.Transaction.PaymentMethod != null)
                .GroupBy(order => order.Transaction.PaymentMethod!.Name)
                .Select(group => new
                {
                    PaymentMethodName = group.Key,
                    OrderCount = group.Count()
                })
                .ToListAsync();

            return stats;
        }
    }
}
