namespace GameCatalouge.Web.Controllers
{
    using GameCatalogue.Services.Data.Interfaces;
    using GameCatalogue.Web.Infrastructure;
	using GameCatalouge.Web.ViewModels.Developer;
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
                this.TempData[ErrorMessage] = "You are already a developer";
                return this.RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Become(BecomeDeveloperFormModel model)
        {
			string? userId = this.User.GetId();
			bool isDeveloper = await this.developerService.DeveloperExistsByUserId(userId!);
			if (isDeveloper)
			{
				this.TempData[ErrorMessage] = "You are already a developer";
				return this.RedirectToAction("Index", "Home");
			}

            bool isEmailalreadyTaken = await this.developerService.DeveloperExistsByEmail(model.BusinessEmail);

            if (isEmailalreadyTaken)
            {
                this.ModelState.AddModelError(nameof(model.BusinessEmail), "A developer with the provided e-mail already exists");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
				await this.developerService.Create(userId!, model);
			}
            catch (Exception)
            {

                this.TempData[ErrorMessage] = "Unexpected error occured during the operation! Please try again later or contact us!";
                return this.RedirectToAction("Index", "Home");
            }

            return this.RedirectToAction("Index", "Home");
		}
    }
}
