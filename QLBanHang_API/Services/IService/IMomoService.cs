using PBL6_BackEnd.Request;
using PBL6_BackEnd.Response;

namespace PBL6_BackEnd.Services.Service
{
    public interface IMomoService
    {
        public Task<MomoOneTimePaymentCreateLinkResponse> CreatePaymentAsync(VnPaymentRequestModel model);
        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
        public string ComputeHmacSha256(string message, string secretKey);
    }
}
