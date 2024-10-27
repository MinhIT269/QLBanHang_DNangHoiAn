using Microsoft.AspNetCore.Mvc;
using PBL6_BackEnd.Services.Service;
using PBL6_QLBH.Data;
using Microsoft.EntityFrameworkCore;

using PBL6_QLBH.Models;

namespace PBL6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IOrderService _orderService;

        public OrderController(DataContext context,IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }


        [HttpGet("getOrderByStatus")]
        public async Task<ActionResult<List<Order>>> GetOrderByStatus([FromQuery] string status, [FromQuery] int page = 1, [FromQuery] int size = 10 )
        {
            int skip = (page - 1) * size;
            var orders = await _orderService.getOrderByStatus(status)
                            .Skip(skip) // Bỏ qua các sản phẩm của các trang trước
                            .Take(size) // Lấy số lượng tương ứng với 'size'
                            .ToListAsync(); // Thực thi truy vấn và trả về danh sách


            return Ok(orders);
        }
    }
}
