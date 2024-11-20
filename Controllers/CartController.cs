﻿using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL6.Dto;
using PBL6.Services.IService;
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
        private readonly ICartService _cartService;

        public CartController(DataContext context, IVnPayService vnPayService,
            IMomoService momoService, ZaloPayConfig zaloPayConfig,
            IOrderService orderService, IOrderDetailService orderDetailService,
            ITransactionService transactionService, IProductService productService,
            IPromotionService promotionService, ICartService cartService)
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
            _cartService = cartService;
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
                if(newOrder.PromotionId == null)
                {
                    Console.WriteLine("id is null");
                }
                var savedOrder = await _orderService.AddOrderWithDetailsAsync(newOrder,newOrder.PromotionId.ToString());
                Console.WriteLine("promotionId:" + newOrder.PromotionId);



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
                    Ammount = (int)(Math.Floor(savedOrder.TotalAmount )),
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


        [HttpGet("PaymentBack")]

        public async Task<IActionResult> PaymentCallBack()
        {

            var response = _vnPayService.PaymentExecute(Request.Query);
            string htmlContent;

            if (response == null || response.VnPayResponseCode != "00")
            {
                htmlContent = GeneratePaymentHtml("Giao dịch thất bại", "Không xác định", 0, "Thất bại", "failed");
                return Content(htmlContent, "text/html");
            }

            var orderId = Guid.Parse(response.OrderDescription);



            var order = await _orderService.GetOrderByIdAsync(orderId);

            var transaction = await _transactionService.GetTransactionByOrderIdAsync(order.OrderId);


            order.Status = "done";
            transaction.Status = "done";

            await _transactionService.SaveChangeAsync();


            htmlContent = GeneratePaymentHtml("Giao dịch thành công", transaction.TransactionId.ToString(), transaction.Amount, "Hoàn thành", "success");
            return Content(htmlContent, "text/html");
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

        [HttpPost("addProductToCart")]
        public async Task<ActionResult<CartDto>> AddProductToCart([FromQuery] Guid productId)
        {

            var cart = await _cartService.AddProductToCart(productId);
            return Ok(cart);
        }


        [HttpGet("getCartOfUser")]

        public async Task<ActionResult<CartDto>> getCartOfUser([FromQuery] Guid userId)
        {
            var cart = await _cartService.GetCartOfUser(userId);    

            return Ok(cart);
        }

    }
}