namespace GameCatalouge.Web.Areas.Admin.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	using static GameCatalogue.Common.GeneralConstants;

	[Area(AdminAreaName)]
	[Authorize(Roles = AdminRoleName)]
	public class BaseAdminController : Controller
	{
	}
}
