﻿using Microsoft.EntityFrameworkCore;
using PBL6_BackEnd.Helpers;

using PBL6_BackEnd.Request;
using PBL6_BackEnd.Response;
using PBL6_BackEnd.Services.Service;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6_BackEnd.Services.ServiceImpl
{
    public class VnPayService(IConfiguration config,DataContext context) : IVnPayService
    {
        private readonly IConfiguration _config = config;
        private readonly DataContext _context = context;
       
        public async Task<decimal> CalculateTotalPriceOfAOrder(Order order,string promoteId)
        {
            decimal totalPrice = 0;
            foreach (OrderDetail detail in order.OrderDetails)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == detail.ProductId);

                if (product != null)
                {
                    Console.WriteLine($"Detail Price before update: {detail.UnitPrice}");
                    detail.UnitPrice = product.Price * detail.Quantity;
                    Console.WriteLine($"Detail Price after update: {detail.UnitPrice}");
                    totalPrice += detail.UnitPrice;
                }
                else
                {
                    Console.WriteLine($"Product with ID {detail.ProductId} not found.");
                }
            }
            if (!string.IsNullOrEmpty(promoteId))
            {
                var promotion = await _context.Promotions.FirstOrDefaultAsync(p => p.PromotionId == Guid.Parse(promoteId));
                order.DiscountPercentage = promotion.Percentage;
                _context.SaveChangesAsync();    
                if (promotion != null && promotion.Percentage > 0)
                {
                    decimal discountAmount = totalPrice * (promotion.Percentage / 100);
                    totalPrice -= discountAmount;
                }
            }


            Console.WriteLine($"Total Price: {totalPrice}");
            return totalPrice;
        }

        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.Ammount * 100).ToString()); 

            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:locale"]);


            vnpay.AddRequestData("vnp_OrderInfo",  model.OrderId.ToString());
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            //vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);
            //vnpay.AddRequestData("vnp_TxnRef", tick);

            var isAndroidEmulator = context.Request.Headers["User-Agent"].ToString().Contains("Android");
            var hostUrl = isAndroidEmulator
                ? "https://10.0.2.2:7080/api/Order/PaymentBack"
                : $"{context.Request.Scheme}://{context.Request.Host}/api/Order/PaymentBack";

            vnpay.AddRequestData("vnp_ReturnUrl", hostUrl);
            vnpay.AddRequestData("vnp_TxnRef", tick);
            
//=======
//                ? "http://10.0.2.2:5273/api/Cart/PaymentBack"
//                : $"{context.Request.Scheme}://{context.Request.Host}/Cart/PaymentBack";

//            vnpay.AddRequestData("vnp_ReturnUrl", hostUrl);
//            vnpay.AddRequestData("vnp_TxnRef", tick);

//>>>>>>> master
            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);

            return paymentUrl;

        }

        public VnPaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_TxnRef = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");

            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);

            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Success = false,
                };
            }

            return new VnPaymentResponseModel
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                TxnRef = vnp_TxnRef.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash.ToString(),
                VnPayResponseCode = vnp_ResponseCode.ToString(),
            };

        }
    }
}
