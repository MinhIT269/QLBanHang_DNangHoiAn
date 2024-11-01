using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL6.Services.Service;
using PBL6_BackEnd.Config;
using PBL6_BackEnd.Helpers;
using PBL6_BackEnd.Request;
using PBL6_BackEnd.Services.Service;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly DataContext _context;
        private readonly IMomoService _momoService;
        private readonly ZaloPayConfig _zaloPayConfig;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ITransactionService _transactionService;
        private readonly IProductService _productService;
        private readonly IPromotionService _promotionService;

        public CartController(DataContext context, IVnPayService vnPayService,
            IMomoService momoService, ZaloPayConfig zaloPayConfig,
            IOrderService orderService, IOrderDetailService orderDetailService,
            ITransactionService transactionService, IProductService productService,
            IPromotionService promotionService)
        {
            _vnPayService = vnPayService;
            _context = context;
            _momoService = momoService;
            _zaloPayConfig = zaloPayConfig;
            _orderService = orderService;
            _transactionService = transactionService;
            _orderDetailService = orderDetailService;
            _productService = productService;
            _promotionService = promotionService;
        }





        [HttpPost("checkout")]

        public async Task<IActionResult> CheckOut([FromBody] Order newOrder, [FromQuery] string payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid order data");
            }

            try
            {
                var savedOrder = await _orderService.AddOrderWithDetailsAsync(newOrder);



                var newTransaction = new Transaction
                {
                    TransactionId = Guid.NewGuid(),
                    OrderId = savedOrder.OrderId,
                    TransactionDate = DateTime.Now,
                    Amount = savedOrder.TotalAmount,
                    Status = savedOrder.Status,
                    TransactionDetails = $"Thanh toán cho đơn hàng {newOrder.OrderId} {DateTime.Now}",
                };

                //await _transactionService.AddTransactionAsync(newTransaction);
                var callbackUrl = "http://10.0.2.2:5273/api/Cart/PaymentBack";

                var model = new VnPaymentRequestModel
                {
                    Ammount = (double)newOrder.TotalAmount,
                    CreatedDate = DateTime.Now,
                    Description = $"Thanh toán đơn hàng {DateTime.Now}",
                    FullName = "Ngo Gia Bao",
                    OrderId = newOrder.OrderId,
                     //CallbackUrl = callbackUrl
                };

                return payment switch
                {
                    "VNP" => await HandleVnpPayment(newTransaction, model),
                    "Momo" => await HandleMomoPayment(newTransaction, model),
                    "ZaloPay" => await HandleZaloPayPayment(newTransaction, newOrder),
                    _ => BadRequest("Unsupported payment method")
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

        [HttpGet("PaymentBack")]

        public async Task<IActionResult> PaymentCallBack()
        {

            var response = _vnPayService.PaymentExecute(Request.Query);
            var orderId = Guid.Parse(response.OrderDescription);



            var order = await _orderService.GetOrderByIdAsync(orderId);

            var transaction = await _transactionService.GetTransactionByOrderIdAsync(order.OrderId);


            order.Status = "done";
            transaction.Status = "done";

            await _transactionService.SaveChangeAsync();


            if (response == null || response.VnPayResponseCode != "00")
            {
                return NotFound();
            }

            return Ok(new { payment = response.Success });
        }



        private async Task<IActionResult> HandleVnpPayment(Transaction transaction, VnPaymentRequestModel model)
        {
            transaction.PaymentMethodId = Guid.Parse("bb1d7cd7-e567-4f2d-b7a7-68e32cf5cf93");
            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, model);
            await _transactionService.AddTransactionAsync(transaction);
            return Ok(paymentUrl);
        }

        private async Task<IActionResult> HandleMomoPayment(Transaction transaction, VnPaymentRequestModel model)
        {
            transaction.PaymentMethodId = Guid.Parse("27e5a56c-d0f5-4ac9-bb41-f1ad59ef67b6");
            var response = await _momoService.CreatePaymentAsync(model);
            await _transactionService.AddTransactionAsync(transaction);
            return Ok(response.payUrl);
        }

        private async Task<IActionResult> HandleZaloPayPayment(Transaction transaction, Order newOrder)
        {
            transaction.PaymentMethodId = Guid.Parse("037e0872-b46f-4040-9632-e2f70796eb43");

            var zaloPayRequest = new CreateZalopayPayRequest(
                _zaloPayConfig.AppId, _zaloPayConfig.AppUser, DateTime.Now.GetTimeStamp(),
                (long)newOrder.TotalAmount, $"{DateTime.Now:yyMMdd}_{newOrder.OrderId}",
                "zalopayapp", "");

            zaloPayRequest.MakeSignature(_zaloPayConfig.Key1);

            var (isSuccess, message) = zaloPayRequest.GetLink(_zaloPayConfig.PaymentUrl);

            if (isSuccess)
            {
                await _transactionService.AddTransactionAsync(transaction);
                return Ok(message);
            }

            return BadRequest("ZaloPay payment failed");
        }

       

        [HttpGet("getAllProduct")]
        public async Task<ActionResult<List<Product>>> GetAllProduct([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            int skip = (page - 1) * size;

            var products = await _productService.getAllProduct()
                .Skip(skip) 
                .Take(size)
                .ToListAsync(); 

            return Ok(products);
        }


        [HttpGet("getAllPromotion")]
        public async Task<ActionResult<List<Promotion>>> GetAllPromotion([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            int skip = (page - 1) * size;

            var promotion = await _promotionService.getAllPromotion()
                .Skip(skip) 
                .Take(size) 
                .ToListAsync(); 



            return Ok(promotion);
        }
    }
}