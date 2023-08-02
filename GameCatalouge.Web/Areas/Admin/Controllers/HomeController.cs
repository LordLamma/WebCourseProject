using Microsoft.AspNetCore.Mvc;

namespace GameCatalouge.Web.Areas.Admin.Controllers
{
	public class HomeController : BaseAdminController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
