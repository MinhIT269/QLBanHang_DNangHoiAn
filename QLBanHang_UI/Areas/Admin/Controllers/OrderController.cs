using Microsoft.AspNetCore.Mvc;
using QLBanHang_UI.Helpers;
using QLBanHang_UI.Models;
using Rotativa.AspNetCore;

namespace QLBanHang_UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class OrderController : Controller
	{
        private readonly ApiHelper _apiHelper;
        private readonly IHttpClientFactory _httpClientFactory;
        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _apiHelper = new ApiHelper(httpClientFactory);
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult IndexOrder()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> DetailOrder(Guid id)
		{
            var order = await _apiHelper.GetDataFromApi<OrderDto>(
                $"https://localhost:7069/api/Order/OrderDetail/{id.ToString()}",
                "Khong the tai đơn hàng");
            return View(order);
		}

        [HttpGet]
        public async Task<IActionResult> PDF(Guid id)
        {
            var order = await _apiHelper.GetDataFromApi<OrderDto>(
                 $"https://localhost:7069/api/Order/OrderDetail/{id.ToString()}",
                 "Khong the tai đơn hàng");
            // Trả về PDF dưới dạng ViewAsPdf
            return new ViewAsPdf(order);
           /* var pdfResult = new ViewAsPdf(order)
            {
                FileName = $"Order_{order.UserInfo.UserName}.pdf"
            };

            return pdfResult;*/
        }
	}
}
