using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using QLBanHang_API.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using QLBanHang_API.Dto;
using System.Numerics;
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
            if (!string.IsNullOrEmpty(username))
            {
                var orders = await dbContext.Orders.Include(x => x.User)
                .Include(x => x.Promotion).AsQueryable()
                .Where(x=> x.User!.UserName == username)
                .Select(order => new Order
                {
                    OrderId = order.OrderId,
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    PromotionId = order.PromotionId ?? Guid.Empty
                }).ToListAsync();
                return orders;
            }
            return new List<Order>();
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

        //Create Order
        public async Task<Order> CreateOrderAsync(Order order)
        {
            await dbContext.Orders.AddAsync(order); 
            await dbContext.SaveChangesAsync();
            return order;
        }
        public async Task<List<OrderDetail>> CreateOrderDetailsAsync(List<OrderDetail> orderDetails)
        {
            try
            {
                await dbContext.OrderDetails.AddRangeAsync(orderDetails);
                foreach(var order in orderDetails)
                {
                    var product = await dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == order.ProductId);
                    if (product != null)
                    {
                        product.Stock = product.Stock - order.Quantity;
                    }
                }
                await dbContext.SaveChangesAsync();
                return orderDetails;
            }
            catch (Exception ex)
            {
                throw new Exception("Does not Add to Database");
            }
        }
        public async Task AddOrderAsync(Order order)
        {
            await dbContext.Orders.AddAsync(order);
        }
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }


        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await dbContext.Orders
         .Include(o => o.User)
         .Include(o => o.Promotion)
         .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
         .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }
    }
}
