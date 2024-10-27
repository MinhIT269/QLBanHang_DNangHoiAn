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
    }
}
