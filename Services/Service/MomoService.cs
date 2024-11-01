using Newtonsoft.Json;
using PBL6_BackEnd.Config;
using PBL6_BackEnd.Request;
using PBL6_BackEnd.Response;
using PBL6_BackEnd.Services.Service;
using RestSharp;
using System.Security.Cryptography;
using System.Text;

namespace PBL6_BackEnd.Services.ServiceImpl
{
    public class MomoService(IConfiguration config) : IMomoService
    {
        private readonly IConfiguration _config=config;

        public string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }

            
        public async Task<MomoOneTimePaymentCreateLinkResponse> CreatePaymentAsync(VnPaymentRequestModel model)
        {
           

            var rawData =
      $"partnerCode={_config["Momo:PartnerCode"]}&accessKey={_config["Momo:AcessKey"]}&requestId={model.OrderId}&amount={model.Ammount}&orderId={model.OrderId}&orderInfo={model.OrderId}&returnUrl={_config["Momo:ReturnUrl"]}&notifyUrl={_config["Momo:NotifyUrl"]}&extraData=";

            var signatures = ComputeHmacSha256(rawData, _config["Momo:SceretKey"]);

            var client = new RestClient(_config["Momo:MomoApiUrl"]);
            var request = new RestRequest() { Method = Method.Post };
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            var requestData = new
            {
                accessKey = _config["Momo:AcessKey"],
                partnerCode = _config["Momo:PartnerCode"],
                requestType = _config["Momo:RequestType"],
                notifyUrl = _config["Momo:NotifyUrl"],
                returnUrl = _config["Momo:ReturnUrl"],
                orderId = model.OrderId.ToString(),
                amount = model.Ammount.ToString(),
                orderInfo = model.OrderId.ToString(),
                requestId = model.OrderId.ToString(),
                extraData = "",
                signature = signatures
            };

            request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            Console.WriteLine("Response Content: " + response.Content);

            return JsonConvert.DeserializeObject<MomoOneTimePaymentCreateLinkResponse>(response.Content);
        }

        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.First(s => s.Key == "amount").Value;
            var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
            var orderId = collection.First(s => s.Key == "orderId").Value;
            return new MomoExecuteResponseModel()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo
            };
        }
    }
}
