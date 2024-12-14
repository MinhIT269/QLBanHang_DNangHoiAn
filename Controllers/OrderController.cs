using Microsoft.AspNetCore.Mvc;
using PBL6_BackEnd.Services.Service;
using PBL6_QLBH.Data;
using Microsoft.EntityFrameworkCore;

using PBL6_QLBH.Models;
using PBL6_BackEnd.Services.ServiceImpl;

namespace PBL6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }


        [HttpGet("getOrderByStatus")]
        public async Task<ActionResult<List<Order>>> GetOrderByStatus([FromQuery] string status, [FromQuery] int page = 1, [FromQuery] int size = 10 )
        {

            int skip = (page - 1) * size;
            var orders = await orderService.getOrderByStatus(status)
                            .Skip(skip) 
                            .Take(size) 
                            .ToListAsync(); 


            return Ok(orders);
        }


        [HttpGet]
        [Route("AllOrder")]
        public async Task<IActionResult> GetAllOrder([FromQuery] Guid id, [FromQuery] string? searchQuery, [FromQuery] int page = 1, int pageSize = 5)
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
            var totalOrdersSuccess = await orderService.TotalOrdersSuccess();
            var totalOrdersCancel = await orderService.TotalOrdersCancel();

            return Ok(new
            {
                TotalOrders = totalOrders,
                OrdersPending = totalOrdersPending,
                OrderSuccess = totalOrdersSuccess,
                OrderCancel = totalOrdersCancel
            });
        }


        [HttpGet("CheckMissionForBeginner")]
        public async Task<IActionResult> CheckMissionForBeginner([FromQuery] string userId)
        {
            return Ok(orderService.CheckMissionForBeginner(Guid.Parse(userId)));
        }
    }
}
