
using PBL6_QLBH.Models;
using QLBanHang_API.Dto.Request;
using QLBanHang_API.Dto.Response;

namespace QLBanHang_API.Services.IService
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext httpContext, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
        public Task<decimal> CalculateTotalPriceOfAOrder(Order order, string promoteId);
    }
}
