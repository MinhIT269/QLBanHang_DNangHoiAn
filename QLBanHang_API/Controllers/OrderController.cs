using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanHang_API.Services.IService;

namespace QLBanHang_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        // URL - /api/Order/AllOrder/username
        [HttpGet]
        [Route("AllOrder")]
        public async Task<IActionResult> GetAllOrder([FromQuery] Guid id, [FromQuery] string? searchQuery,[FromQuery] int page = 1, int pageSize = 5)
        {
            try
            {
                // Gọi service để lấy danh sách đơn hàng
                var ordersDTO = await orderService.GetAllOrders(id, searchQuery, page, pageSize);

                // Kiểm tra nếu không có đơn hàng nào được tìm thấy
                if (ordersDTO == null || !ordersDTO.Any())
                {
                    return NotFound(new { Message = "No orders found for the given user ID and search query." });
                }

                // Trả về danh sách đơn hàng
                return Ok(ordersDTO);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi không mong muốn
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Details = ex.Message });
            }
        }
        [HttpGet("TotalPagesOrdered_Detail")]
        public async Task<IActionResult> GetTotalPagesCategory_Detail([FromQuery] Guid id, [FromQuery] string searchQuery = "")
        {
            var ordersDTO = await orderService.GetAllOrders(id, searchQuery);
            // Check if the list is null or empty
            if (ordersDTO == null || !ordersDTO.Any())
            {
                // Return 0 pages if no orders are found
                return Ok(0);
            }

            // Get the total number of records
            var totalRecords = ordersDTO.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / 5); // Điều chỉnh số item trên mỗi trang nếu cần
            return Ok(totalPages);
        }

        [HttpGet("GetOrdersStats_UserDetail")]
        public async Task<IActionResult> GetOrdersStats_UserDetail([FromQuery] Guid id)
        {
            var totalOrders = await orderService.TotalOrdersByUser(id);
            var totalOrdersPending = await orderService.TotalOrdersPendingByUser(id);
            var totalOrdersSuccess = await orderService.TotalOrdersSuccessByUser(id);
            var SumOrder = await orderService.SumCompletedOrdersAmountByUser(id);

            return Ok(new
            {
                TotalOrders = totalOrders,
                OrdersPending = totalOrdersPending,
                OrderSuccess = totalOrdersSuccess,
                SumOrder = SumOrder
            });
        }

        // URL - /api/Order/OrderDetail/id
        [HttpGet]
        [Route("OrderDetail/{id:guid}")]
        public async Task<IActionResult> GetOrderDetail([FromRoute] Guid id)
        {
            var orderDetailDTO = await orderService.GetOrderDetails(id);
            if (orderDetailDTO == null)
            {
                return NotFound();
            }
            return Ok(orderDetailDTO);
        }

        // URL - /api/Order/OrderUpdate?id=?&status=?
        [HttpPut]
        [Route("OrderUpdate")]
        public async Task<IActionResult> UpdateOrder([FromQuery] Guid id, [FromQuery] string status)
        {
            var orderDTO = await orderService.UpdateOrder(id, status);
            if (orderDTO == null)
            {
                return NotFound();
            }

            return Ok(orderDTO);
        }

        [HttpGet("GetFilterOrdered")]
        public async Task<IActionResult> GetFilteredCategories([FromQuery] string searchQuery = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 8, [FromQuery] string sortCriteria = "name", [FromQuery] bool isDescending = false)
        {
            var orders = await orderService.GetFilteredOrders(page, pageSize, searchQuery, sortCriteria, isDescending);

            return Ok(orders);
        }

        [HttpGet("TotalPagesOrdered")]
        public async Task<IActionResult> GetTotalPagesCategory([FromQuery] string searchQuery = "")
        {
            var totalRecords = await orderService.GetTotalOrdersAsync(searchQuery);
            var totalPages = (int)Math.Ceiling((double)totalRecords / 8); // Điều chỉnh số item trên mỗi trang nếu cần
            return Ok(totalPages);
        }

        [HttpGet("TotalOrders")]
        public async Task<IActionResult> TotalOrders()
        {
            var totalOrders = await orderService.TotalOrders();
            return Ok(totalOrders);
        }

        [HttpGet("GetOrdersStats")]
        public async Task<IActionResult> GetProductStats()
        {
            var totalOrders = await orderService.TotalOrders();
            var totalOrdersPending = await orderService.TotalOrdersPending();
            var totalOrdersSuccess= await orderService.TotalOrdersSuccess();
            var totalOrdersCancel = await orderService.TotalOrdersCancel();

            return Ok(new
            {
                TotalOrders = totalOrders,
                OrdersPending = totalOrdersPending,
                OrderSuccess = totalOrdersSuccess,
                OrderCancel = totalOrdersCancel
            });
        }
    }
}
