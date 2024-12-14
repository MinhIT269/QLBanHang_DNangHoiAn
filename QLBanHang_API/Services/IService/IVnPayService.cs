using PBL6_BackEnd.Request;
using PBL6_BackEnd.Response;
using PBL6_QLBH.Models;

namespace PBL6_BackEnd.Services.Service
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext httpContext, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
        public Task<decimal> CalculateTotalPriceOfAOrder(Order order,string promoteId);
    }
}
