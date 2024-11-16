using Microsoft.AspNetCore.Mvc;

namespace QLBanHang_UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PurchaseController : Controller
    {
        public IActionResult IndexPurchase()
        {
            return View();
        }
    }
}
