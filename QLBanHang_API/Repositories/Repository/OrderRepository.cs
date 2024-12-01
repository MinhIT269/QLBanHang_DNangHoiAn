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

        public IQueryable<Order> GetFilteredOrders(string searchQuery, string sortCriteria, bool isDescending)
        {
            var query = dbContext.Orders
                .Include(c => c.User)
                .Include(c => c.OrderDetails)!
                .ThenInclude(p => p.Product).AsQueryable();
            
            if(!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => EF.Functions.Collate(c.User!.UserName, "SQL_Latin1_General_CP1_CI_AI")!.Contains(searchQuery));
            }

            // Áp dụng sắp xếp
            query = sortCriteria switch
            {
                "name" => isDescending ? query.OrderByDescending(c => c.User!.UserName) : query.OrderBy(c => c.User!.UserName),
                "totalAmount" => isDescending ? query.OrderByDescending(c => c.TotalAmount) : query.OrderBy(c => c.TotalAmount),
                "createDate" => isDescending ? query.OrderByDescending(c => c.OrderDate) : query.OrderBy(c => c.OrderDate),
                "status" => isDescending ? query.OrderByDescending (c => c.Status) : query.OrderBy(c => c.Status),
                _ => query
            };
            return query;
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

        // Lấy thông tin Order theo OrderId
        public async Task<Order> GetOrderDetailAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return null; // Trả về null nếu id không hợp lệ
            }

            var order = await dbContext.Orders
                .Include(o => o.User)               // Tải thông tin người dùng
                    .ThenInclude(u => u.UserInfo)
                .Include(o => o.OrderDetails!)       // Tải danh sách chi tiết đơn hàng
                    .ThenInclude(od => od.Product)  // Tải thông tin sản phẩm trong OrderDetails (nếu cần)
                .Include(o => o.Promotion)          // Tải thông tin khuyến mãi
                .Include(o => o.Transaction)        // Tải thông tin giao dịch
                .FirstOrDefaultAsync(o => o.OrderId == id); // Lấy đơn hàng đầu tiên khớp với OrderId

            return order!;
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
        public async Task<int> TotalOrders()
        {
            return await dbContext.Orders.CountAsync();
        }
		public async Task<int> TotalOrdersSuccess()
		{
			return await dbContext.Orders.Where(c => c.Status == "Completed").CountAsync();
		}
		public async Task<int> TotalOrdersPending()
		{
			return await dbContext.Orders.Where(c => c.Status == "Pending").CountAsync();
		}
		public async Task<int> TotalOrdersCancel()
		{
			return await dbContext.Orders.Where(c => c.Status == "Cancel").CountAsync();
		}
	}
}
