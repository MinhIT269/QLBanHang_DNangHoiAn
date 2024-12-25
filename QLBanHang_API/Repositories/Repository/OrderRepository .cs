using Microsoft.EntityFrameworkCore;
using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using System.Globalization;


namespace PBL6.Repositories.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext dbContext;

        public OrderRepository(DataContext context)
        {
            dbContext= context;
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
           .ThenInclude(u => u.UserInfo)
         .Include(o => o.Promotion)
         .Include(o => o.OrderDetails) 
            .ThenInclude(od => od.Product)
         .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public IQueryable<Order> GetOrdersByStatus(string status)
        {
            #pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
                return dbContext.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                      .ThenInclude(p => p.Brand)
                        .ThenInclude(b => b.Locations)
                .Where(o => o.Status == status);
            #pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        }


        public IQueryable<Order> GetFilteredOrders(string searchQuery, string sortCriteria, bool isDescending)
        {
            var query = dbContext.Orders
                .Include(c => c.User)
                .Include(c => c.OrderDetails)!
                .ThenInclude(p => p.Product).AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => EF.Functions.Collate(c.User!.UserName, "SQL_Latin1_General_CP1_CI_AI")!.Contains(searchQuery));
            }

            // Áp dụng sắp xếp
            query = sortCriteria switch
            {
                "name" => isDescending ? query.OrderByDescending(c => c.User!.UserName) : query.OrderBy(c => c.User!.UserName),
                "totalAmount" => isDescending ? query.OrderByDescending(c => c.TotalAmount) : query.OrderBy(c => c.TotalAmount),
                "createDate" => isDescending ? query.OrderByDescending(c => c.OrderDate) : query.OrderBy(c => c.OrderDate),
                "status" => isDescending ? query.OrderByDescending(c => c.Status) : query.OrderBy(c => c.Status),
                _ => query
            };
            return query;
        }

        // Get All Order By Username 
        public async Task<List<Order>> GetAllOrderAsync(Guid? id, string searchQuery)
        {
            if (id != null)
            {
                var query = dbContext.Orders
                    .Include(x => x.User)
                    .Include(x => x.Promotion)
                    .AsQueryable();

                // Lọc theo UserId
                query = query.Where(x => x.UserId == id);

                // Lọc theo searchQuery (nếu không null hoặc rỗng)
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query = query.Where(order =>
                        order.OrderId.ToString().Substring(0, 8).Contains(searchQuery));
                }

                // Lấy danh sách kết quả
                var orders = await query.Select(order => new Order
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
                .Include(o => o.OrderDetails!)      // Tải danh sách chi tiết đơn hàng
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
            return await dbContext.Orders.Where(c => c.Status == "completed").CountAsync();
        }
        public async Task<int> TotalOrdersPending()
        {
            return await dbContext.Orders.Where(c => c.Status == "Pending").CountAsync();
        }
        public async Task<int> TotalOrdersCancel()
        {
            return await dbContext.Orders.Where(c => c.Status == "Cancel").CountAsync();
        }
        private Boolean CheckMission1_Complete3Orders()
        {
            DateTime now = DateTime.Now;
            var  orders = dbContext.Orders
              .Where(order => order.OrderDate.HasValue 
                    && order.OrderDate.Value.Year == now.Year 
                    && order.OrderDate.Value.Month == now.Month 
                    && order.Status == "completed")
                .Take(3)
                .ToList();
            if(orders.Count == 3 ) { return true; }
            return false;
        }


        private Boolean CheckMission2_ReviewMission(Guid userId)
        {
             var reviews = dbContext.Reviews
            .Where(review => review.UserId == userId) 
            .Take(2) 
            .ToList();

            if(reviews.Count >= 2 ) { return true; }    
            return false;
        }

        private Boolean CheckMission3_OrderedAtLeast3DiffrentProduct(Guid userId)
        {
            var purchasedProducts = dbContext.Orders
           .Where(order => order.UserId == userId && order.Status == "completed")
           .SelectMany(order => order.OrderDetails) 
           .Select(detail => detail.ProductId)
           .Distinct() 
           .ToList();

            if(purchasedProducts.Count >= 3 ) { return true; }
            return false;
        }

        private Boolean CheckMission4_AppliedDiscountFor2Product(Guid userId)
        {
            var discountedProducts = dbContext.Products
                .Where(product => product.PromotionPrice.HasValue && product.PromotionPrice.Value > 0)
                .Take(2)
                .ToList();

            if (discountedProducts.Count >= 2) { return true; }
            return false;
        }

        private Boolean CheckMission5_SpentAtLeast3500000ThisMonth(Guid userId)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var totalSpent = dbContext.Orders
                .Where(order => order.UserId == userId &&
                                order.OrderDate.HasValue &&
                                order.OrderDate.Value.Month == currentMonth &&
                                order.OrderDate.Value.Year == currentYear)
                .Sum(order => order.TotalAmount);

            return totalSpent >= 350000;
        }


        public Task<object> MissionForBeginnerStatus(Guid userId)
        {

            var missionStatus = new Dictionary<string, string>
                {
                    { "mission 1", CheckMission1_Complete3Orders() ? "completed" : "not completed" },
                    { "mission 2", CheckMission2_ReviewMission(userId) ? "completed" : "not completed" },
                    { "mission 3", CheckMission3_OrderedAtLeast3DiffrentProduct(userId) ? "completed" : "not completed" },
                    { "mission 4", CheckMission4_AppliedDiscountFor2Product(userId) ? "completed" : "not completed" },
                    { "mission 5" , CheckMission5_SpentAtLeast3500000ThisMonth(userId) ? "completed" : "not completed" }
            };
            return Task.FromResult<object>(missionStatus);
        }

        public async Task<int> TotalOrdersByUser(Guid userId)
        {
            return await dbContext.Orders
                .Where(o => o.UserId == userId)
                .CountAsync();
        }

        public async Task<int> TotalOrdersSuccessByUser(Guid userId)
        {
            return await dbContext.Orders
                .Where(o => o.UserId == userId && o.Status == "completed")
                .CountAsync();
        }

        public async Task<int> TotalOrdersPendingByUser(Guid userId)
        {
            return await dbContext.Orders
                .Where(o => o.UserId == userId && (o.Status == "Pending" || o.Status == "Cancel"))
                .CountAsync();
        }
        public async Task<decimal> SumCompletedOrdersAmountByUser(Guid userId)
        {
            return await dbContext.Orders
                .Where(o => o.UserId == userId && o.Status == "completed")
                .SumAsync(o => o.TotalAmount);
        }
        public async Task<decimal> GetTotalAmountOfCompletedOrdersAsync()
        {
            return await dbContext.Orders
                .Where(o => o.Status == "completed") 
                .SumAsync(o => o.TotalAmount); 
        }


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
                await dbContext.SaveChangesAsync();
                return orderDetails;
            }
            catch (Exception ex)
            {
                throw new Exception("Does not Add to Database");
            }
        }
        public async Task<Dictionary<string, decimal>> GetOrderStatistics(string period)
        {
            var now = DateTime.Now;
            IQueryable<Order> query = dbContext.Orders.Where(o => o.Status == "completed"); // Lọc chỉ đơn hàng thành công

            var statistics = new Dictionary<string, decimal>();

            switch (period.ToLower())
            {
                case "week":
                    var startOfWeek = now.AddDays(-(int)now.DayOfWeek);
                    var ordersThisWeek = query
                        .Where(o => o.OrderDate >= startOfWeek)
                        .AsEnumerable()
                        .GroupBy(o => o.OrderDate.Value.DayOfWeek)
                        .Select(g => new { Day = g.Key, TotalAmount = g.Sum(o => o.TotalAmount) })
                        .ToList();

                    foreach (var day in Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>())
                    {
                        statistics[day.ToString()] = ordersThisWeek.FirstOrDefault(o => o.Day == day)?.TotalAmount ?? 0;
                    }
                    break;

                case "month":
                    var ordersThisMonth = query
                        .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Month == now.Month && o.OrderDate.Value.Year == now.Year)
                        .AsEnumerable()
                        .GroupBy(o => o.OrderDate.Value.Day)
                        .Select(g => new { Day = g.Key, TotalAmount = g.Sum(o => o.TotalAmount) })
                        .ToList();

                    for (int i = 1; i <= DateTime.DaysInMonth(now.Year, now.Month); i++)
                    {
                        statistics[i.ToString()] = ordersThisMonth.FirstOrDefault(o => o.Day == i)?.TotalAmount ?? 0;
                    }
                    break;

                case "year":
                    var ordersThisYear = query
                        .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == now.Year)
                        .AsEnumerable()
                        .GroupBy(o => o.OrderDate.Value.Month)
                        .Select(g => new { Month = g.Key, TotalAmount = g.Sum(o => o.TotalAmount) })
                        .ToList();

                    for (int i = 1; i <= 12; i++)
                    {
                        statistics[CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i)] = ordersThisYear.FirstOrDefault(o => o.Month == i)?.TotalAmount ?? 0;
                    }
                    break;

                default:
                    throw new ArgumentException("Invalid period. Allowed values are 'week', 'month', or 'year'.");
            }

            return statistics;
        }
        public async Task UpdateProductAfterSucessAsync(Order order)
        {
            var orderDetails = await dbContext.OrderDetails.Where(x => x.OrderId == order.OrderId).ToListAsync();
            foreach (var item in orderDetails)
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                if (product != null)
                {
                    product.Stock = product.Stock - item.Quantity;
                }
            }
            await dbContext.SaveChangesAsync();

        }
    }
}
