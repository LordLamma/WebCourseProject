namespace GameCatalouge.Web.Controllers
{
    using GameCatalogue.Services.Data.Interfaces;
    using GameCatalouge.Web.ViewModels.Home;
	using static GameCatalogue.Common.GeneralConstants;
	using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    public class HomeController : Controller
    {
        private readonly IGameService gameService;

        public HomeController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        public async Task<IActionResult> Index()
        {
            if (this.User.IsInRole(AdminRoleName))
            {
                return this.RedirectToAction("Index", "Home", new { Area = AdminAreaName });
            }
            IEnumerable<IndexViewModel> viewModel =
                await this.gameService.LastThreeGames();
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404)
            {
                return this.View("Error404");
            }
            return View();
        }
    }
}