using Microsoft.AspNetCore.Mvc;

namespace QLBanHang_UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PromotionController : Controller
    {
        public IActionResult IndexPromotion()
        {
            return View();
        }
        public IActionResult CreatePromotion()
        {
            return View();
        }
    }
}
