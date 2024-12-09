using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto.Request;
using QLBanHang_API.Service;
using QLBanHang_API.Services.IService;
using QLBanHang_API.Services.Service;

namespace QLBanHang_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IVnPayService vnPayService;
        private readonly DataContext dataContext;
        private readonly IProductService productService;
        private readonly IPromotionService promotionService;
        private readonly ITransactionService transactionService;

        public OrderController(IOrderService orderService, IPromotionService promotionService, ITransactionService transactionService
            , IProductService productService, IVnPayService vnPayService, DataContext dataContext)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.promotionService = promotionService;
            this.transactionService = transactionService;
            this.vnPayService = vnPayService;
            this.dataContext = dataContext;
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

        //Create Order
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest order)
        {
            var orderDto = await orderService.CreateOrder(order);
            if (orderDto == null)
            {
                return BadRequest();
            }
            return Ok(orderDto);
        }
        [HttpPost]
        [Route("CreateOrderDetail")]
        public async Task<IActionResult> CreateOrderDetail([FromBody] List<OrderDetailsRequest> orderDetails)
        {
            var orderDetail = await orderService.CreateOrderDetail(orderDetails);
            if (orderDetails == null)
            {
                return BadRequest();
            }
            return Ok(orderDetail);
        }
        [HttpPost("checkout")]

        public async Task<IActionResult> CheckOut([FromBody] OrderRequest orderRequest, [FromQuery] string payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid order data");
            }
            try
            {
                if (orderRequest.PromotionId == null)
                {
                    Console.WriteLine("id is null");
                }
                var savedOrder = await orderService.CreateOrder(orderRequest);



                var newTransaction = new Transaction
                {
                    TransactionId = Guid.NewGuid(),
                    OrderId = savedOrder.OrderId,
                    TransactionDate = DateTime.Now,
                    Amount = savedOrder.TotalAmount,
                    Status = savedOrder.Status,
                    TransactionDetails = $"Thanh toán cho đơn hàng {orderRequest.OrderId} {DateTime.Now}",
                };

                var callbackUrl = "https://10.0.2.2:7069/api/Cart/PaymentBack";

                var model = new VnPaymentRequestModel
                {
                    Ammount = (int)(Math.Floor(savedOrder.TotalAmount)),
                    CreatedDate = DateTime.Now,
                    Description = $"Thanh toán đơn hàng {DateTime.Now}",
                    FullName = "Ngo Gia Bao",
                    OrderId = orderRequest.OrderId,
                };

                return payment switch
                {
                    "VNP" => await HandleVnpPayment(newTransaction, model),
                    _ => BadRequest("Unsupported payment method")
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }
        private string GeneratePaymentHtml(string message, string transactionId, decimal amount, string statusText, string statusClass)
        {
            return $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Kết quả thanh toán</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                display: flex;
                align-items: center;
                justify-content: center;
                height: 100vh;
                margin: 0;
                background-color: #f0f2f5;
            }}
            .payment-result-container {{
                text-align: center;
                padding: 2rem;
                border-radius: 8px;
                max-width: 400px;
                background-color: #fff;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            }}
            .payment-status.success {{
                color: #4CAF50;
            }}
            .payment-status.failed {{
                color: #E74C3C;
            }}
            .payment-details {{
                margin-top: 1rem;
                font-size: 1rem;
                color: #333;
            }}
            .return-button {{
                margin-top: 1.5rem;
                display: inline-block;
                padding: 0.6rem 1.2rem;
                font-size: 1rem;
                color: #fff;
                background-color: #3498DB;
                text-decoration: none;
                border-radius: 4px;
                transition: background-color 0.3s;
            }}
            .return-button:hover {{
                background-color: #2980B9;
            }}
        </style>
    </head>
    <body>

    <div class='payment-result-container'>
        <h1 class='payment-status {statusClass}'>{message}</h1>
        <div class='payment-details'>
            <p><strong>Mã giao dịch:</strong> {transactionId}</p>
            <p><strong>Số tiền:</strong> {amount} VND</p>
            <p><strong>Trạng thái:</strong> {statusText}</p>
        </div>
        <a href='/' class='return-button'>Quay về trang chủ</a>
    </div>

    </body>
    </html>";
        }

        private async Task<IActionResult> HandleVnpPayment(Transaction transaction, VnPaymentRequestModel model)
        {
            transaction.PaymentMethodId = Guid.Parse("bb1d7cd7-e567-4f2d-b7a7-68e32cf5cf93");
            var paymentUrl = vnPayService.CreatePaymentUrl(HttpContext, model);
            await transactionService.AddTransactionAsync(transaction);
            return Ok(paymentUrl);
        }
        [HttpGet("PaymentBack")]

        public async Task<IActionResult> PaymentCallBack()
        {

            var response = vnPayService.PaymentExecute(Request.Query);
            var orderId = Guid.Parse(response.OrderDescription);

            string htmlContent;
            var order = await orderService.GetOrderByIdAsync(orderId);
            var transaction = await transactionService.GetTransactionByOrderIdAsync(order.OrderId);


            if (response == null || response.VnPayResponseCode != "00")
            {
                htmlContent = GeneratePaymentHtml("Giao dịch thất bại", "Không xác định", 0, "Thất bại", "failed");
                order.Status = "cancelled";
                transaction.Status = "cancelled";
                await orderService.SaveChangeAsync();
                return Content(htmlContent, "text/html");
            }
            else
            {


                order.Status = "done";
                transaction.Status = "done";
                await orderService.UpdateOrderAfterCompleteTransaction(order);
                await transactionService.SaveChangeAsync();
                Console.WriteLine("order date" + order.OrderDate);

                //var emailOrderDetail = new EmailOrderModel
                //{
                //    TotalAmount = order.TotalAmount,
                //    Orders = order.OrderDetails,
                //    OrderDate = order.OrderDate.HasValue ? order.OrderDate.Value : DateTime.Now,
                //    OrderNumber = order.OrderId.ToString(),  // Số hóa đơn
                //    CustomerName = order.User.UserInfo.FirstName + " " + order.User.UserInfo.LastName,  // Tên khách hàng
                //    CustomerAddress = order.User.UserInfo.Address,  // Địa chỉ khách hàng (nếu có)
                //    CustomerPhone = order.User.PhoneNumber,  // Số điện thoại khách hàng (nếu có)
                //    CustomerEmail = order.User.Email,  // Email khách hàng
                //    SellerName = "Hoàn-DNG.INC",  // Tên người bán
                //    SellerAddress = "54 Nguyễn Lương Bằng, Đà Nẵng",  // Địa chỉ người bán
                //    SellerPhone = "0912346789",  // Số điện thoại người bán
                //    SellerEmail = "hoandng@gmail.com",  // Email người bán
                //    Discount = order.Promotion?.Percentage ?? 0,  // Giảm giá (nếu có)
                //    Tax = 0,  // Thuế, nếu có
                //};



                var viewName = "EmailTemplates/OrderConfirmation"; // Tên view của bạn trong Views/EmailTemplates
                //await _emailSender.SendEmailWithViewAsync("baobap2082003@gmail.com", "Xác nhận đơn hàng", viewName, emailOrderDetail);




                htmlContent = GeneratePaymentHtml("Giao dịch thành công", transaction.TransactionId.ToString(), transaction.Amount, "Hoàn thành", "success");
                return Content(htmlContent, "text/html");
            }

        }
    }
}
