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
        [Route("AllOrder/{username}")]
        public async Task<IActionResult> GetAllOrder([FromRoute] string username)
        {
            var ordersDTO = await orderService.GetAllOrders(username);
            if (ordersDTO == null || !ordersDTO.Any())
            {
                return NotFound();
            }

            return Ok(ordersDTO);
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
