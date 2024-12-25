using Microsoft.AspNetCore.Mvc;

namespace QLBanHang_UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
			{
				return View();
			}
			return Error();           
		}
		public IActionResult Error()
		{
			return View();
		}
	}
}
