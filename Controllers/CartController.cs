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

        public async Task<string> CheckOut([FromBody] Order newOrder, [FromQuery] string payment)
        {
            if (ModelState.IsValid)
            {
                newOrder.OrderId = Guid.NewGuid();
                newOrder.TotalAmount = await _vnPayService.CalculateTotalPriceOfAOrder(newOrder);
                newOrder.OrderDate = DateTime.Now;
                newOrder.UserId = Guid.Parse("d4e56743-ff2c-41d3-957d-576e9f574c5d");
                newOrder.PromotionId = Guid.Parse("78913b9e-9d5a-40c4-89ad-813c72d4b1f7");


                //await _context.Orders.AddAsync(newOrder);
                await _orderService.AddOrderAsync(newOrder);
                foreach (OrderDetail detail in newOrder.OrderDetails)
                {

                    //await _context.OrderDetails.AddAsync(detail);
                    await _orderDetailService.AddOrderDetaikAsync(detail);
                }

                //await _context.SaveChangesAsync();
                await _orderService.SaveChangeAsync();




                Transaction newTransaction = new Transaction
                {
                    TransactionId = Guid.NewGuid(),
                    OrderId = newOrder.OrderId,
                    TransactionDate = DateTime.Now,
                    Amount = newOrder.TotalAmount,
                    Status = newOrder.Status,
                    TransactionDetails = "Thanh toan cho don hang " + newOrder.OrderId + " " + DateTime.Now,
                };
                //await _context.Transactions.AddAsync(newTransaction);

                await _transactionService.AddTransactionAsync(newTransaction);

                var Model = new VnPaymentRequestModel
                {
                    Ammount = (double)newOrder.TotalAmount,
                    CreatedDate = DateTime.Now,
                    Description = "Thanh toán đơn hàng " + DateTime.Now.ToString(),
                    FullName = "Ngo Gia Bao",
                    OrderId = newOrder.OrderId,
                };
                if (payment == "VNP")
                {
                    newTransaction.PaymentMethodId = Guid.Parse("bb1d7cd7-e567-4f2d-b7a7-68e32cf5cf93");
                    Console.WriteLine(_vnPayService.CreatePaymentUrl(HttpContext, Model));
                    //await _context.SaveChangesAsync();
                    await _orderService.SaveChangeAsync();
                    return _vnPayService.CreatePaymentUrl(HttpContext, Model);
                }
                else if (payment == "Momo")
                {
                    newTransaction.PaymentMethodId = Guid.Parse("27e5a56c-d0f5-4ac9-bb41-f1ad59ef67b6");

                    var response = await _momoService.CreatePaymentAsync(Model);
                    //await _context.SaveChangesAsync();
                    await _orderService.SaveChangeAsync();
                    Console.WriteLine(response.payUrl);
                    return response.payUrl;
                }

                else if (payment == "ZaloPay")
                {
                    newTransaction.PaymentMethodId = Guid.Parse("037e0872-b46f-4040-9632-e2f70796eb43");
                    var zaloPayRequest = new CreateZalopayPayRequest(
                     _zaloPayConfig.AppId, _zaloPayConfig.AppUser, DateTime.Now.GetTimeStamp(),
                     (long)newOrder.TotalAmount, $"{DateTime.Now:yyMMdd}_{newOrder.OrderId}",
                     "zalopayapp", "");
                    zaloPayRequest.MakeSignature(_zaloPayConfig.Key1);

                    Console.WriteLine("Key1:" + _zaloPayConfig.Key1);
                    Console.WriteLine("App Id:" + _zaloPayConfig.AppId);
                    Console.WriteLine("App User:" + _zaloPayConfig.AppUser);

                    Console.WriteLine("trans id:" + $"{DateTime.Now:yyMMdd}_{newOrder.OrderId}");

                    var (isZaloPaySuccess, zaloPayMessage) = zaloPayRequest.GetLink(_zaloPayConfig.PaymentUrl);

                    if (isZaloPaySuccess)
                    {
                        Console.WriteLine(zaloPayMessage);
                        //await _context.SaveChangesAsync();
                        await _orderService.SaveChangeAsync();
                        return zaloPayMessage;

                    }
                    else
                    {
                        return "err";
                    }
                }



            }
            return "error";

        }

        [HttpGet("PaymentBack")]

        public async Task<IActionResult> PaymentCallBack()
        {

            var response = _vnPayService.PaymentExecute(Request.Query);
            var orderId = Guid.Parse(response.OrderDescription);

            var order = await _context.Orders.FirstOrDefaultAsync(p => p.OrderId == orderId);
            var transaction = await _context.Transactions.FirstOrDefaultAsync(p => p.OrderId == orderId);

            order.Status = "done";
            transaction.Status = "done";

            _context.SaveChanges();


            if (response == null || response.VnPayResponseCode != "00")
            {
                return NotFound();
            }

            return Ok(new { payment = response.Success });
        }

        [HttpGet("getAllProduct")]
        public async Task<ActionResult<List<Product>>> GetAllProduct([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            int skip = (page - 1) * size;

            // Lấy sản phẩm phân trang
            var products = await _productService.getAllProduct()
                .Skip(skip) // Bỏ qua các sản phẩm của các trang trước
                .Take(size) // Lấy số lượng sản phẩm tương ứng với 'size'
                .ToListAsync(); // Thực thi truy vấn và trả về danh sách

            return Ok(products);
        }


        [HttpGet("getAllPromotion")]
        public async Task<ActionResult<List<Promotion>>> GetAllPromotion([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            int skip = (page - 1) * size;

            var promotion = await _promotionService.getAllPromotion()
                .Skip(skip) // Bỏ qua các sản phẩm của các trang trước
                .Take(size) // Lấy số lượng tương ứng với 'size'
                .ToListAsync(); // Thực thi truy vấn và trả về danh sách



            return Ok(promotion);
        }
    }
}