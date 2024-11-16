using Microsoft.AspNetCore.Mvc;

namespace QLBanHang_UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ReviewController : Controller
	{
		public IActionResult IndexReview()
		{
			return View();
		}
	}
}
