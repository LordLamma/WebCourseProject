namespace GameCatalouge.Web.Areas.Admin.Controllers
{
	using GameCatalogue.Services.Data.Interfaces;
	using GameCatalouge.Web.ViewModels.User;
	using Microsoft.AspNetCore.Mvc;


	public class UserController : BaseAdminController
	{
		private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        public async Task<IActionResult> All()
		{
			IEnumerable<AllUserViewModel> viewModel = await this.userService.AllAsync();

			return View(viewModel);
		}
	}
}
