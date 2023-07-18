namespace GameCatalouge.Web.Controllers
{
    using GameCatalogue.Services.Data.Interfaces;
    using GameCatalogue.Web.Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static GameCatalogue.Common.NotificationMessages;


    [Authorize]
    public class DeveloperController : Controller
    {
        private readonly IDeveloperService developerService;

        public DeveloperController(IDeveloperService developerService)
        {
            this.developerService = developerService;
        }

        [HttpGet]
        public async Task<IActionResult> Become()
        {
            string? userId = this.User.GetId();
            bool isDeveloper = await this.developerService.DeveloperExistsByUserId(userId);
            if (isDeveloper)
            {
                TempData[ErrorMessage] = "You are already a developer";
                return this.RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
