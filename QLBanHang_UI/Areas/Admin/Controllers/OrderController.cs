using Microsoft.AspNetCore.Mvc;

namespace QLBanHang_UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class OrderController : Controller
	{
		public IActionResult IndexOrder()
		{
			return View();
		}
	}
}
