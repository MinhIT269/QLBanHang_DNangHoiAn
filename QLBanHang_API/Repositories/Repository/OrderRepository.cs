using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using QLBanHang_API.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
namespace QLBanHang_API.Repositories.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext dbContext;
        public OrderRepository(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Get All Order By Username 
        public async Task<List<Order>> GetAllOrderAsync(string? username)
        {
            var orders = dbContext.Orders.Include("User").AsQueryable();

            if (!string.IsNullOrEmpty(username))
            {
                orders = orders.Where(x => x.User!.Username == username);
            }
            else
            {
                return null; // Trả về danh sách rỗng thay vì null
            }

            return await orders.ToListAsync();
        }


        //Get info Order by OrderID
        public async Task<List<OrderDetail>> GetOrderDetailAsync(Guid? id)
        {
            var orderDetails = dbContext.OrderDetails.Include("Product").AsQueryable();
            if (id == Guid.Empty)
            {
                return null;
            }
            else
            {
                orderDetails = orderDetails.Where(x => x.OrderId == id);
            }
            return await orderDetails.ToListAsync();
        }

        // Update status cua Order
        public async Task<Order> UpdateOrderAsync(Guid id, string status)
        {
            var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == id);
            if (order == null)
            {
                return null;
            }
            order.Status = status;
            await dbContext.SaveChangesAsync();
            return order;

        }
    }
}
