using Microsoft.AspNetCore.Mvc;

namespace QLBanHang_UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        public IActionResult IndexCustomer()
        {
            return View();
        }
    }
}
