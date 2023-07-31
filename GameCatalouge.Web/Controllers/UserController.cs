namespace GameCatalouge.Web.Controllers
{
	using GameCatalogue.Data.Models;
	using GameCatalouge.Web.ViewModels.User;
	using Microsoft.AspNetCore.Authentication;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;

	using static GameCatalogue.Common.NotificationMessages;

	public class UserController : Controller
	{
		private readonly SignInManager<ModdedUser> signInManager;
		private readonly UserManager<ModdedUser> userManager;

        public UserController(SignInManager<ModdedUser> signInManager,
								UserManager<ModdedUser> userManager)
        {
            this.signInManager = signInManager;
			this.userManager = userManager;
        }

        [HttpGet]
		public IActionResult Register()
		{
			return this.View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterFormModel formModel)
		{
			if (!ModelState.IsValid)
			{
				return this.View(formModel);
			}

			ModdedUser user = new ModdedUser();

			await this.userManager.SetUserNameAsync(user, formModel.DisplayName);
			await this.userManager.SetEmailAsync(user, formModel.Email);

			IdentityResult result = await this.userManager.CreateAsync(user, formModel.Password);
			if (!result.Succeeded) 
			{
				foreach (IdentityError error in result.Errors)	
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return this.View(formModel);
			}

			await this.signInManager.SignInAsync(user, isPersistent: false);
			return this.RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public async Task<IActionResult> Login(string returnUrl = null) 
		{
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			LoginFormModel formModel = new LoginFormModel()
			{
				ReturnUrl = returnUrl
			};
			return this.View(formModel);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginFormModel formModel)
		{
			if (!ModelState.IsValid)
			{
				return this.View(formModel);
			}

            var result = 
				await this.signInManager.PasswordSignInAsync(formModel.DisplayName, formModel.Password, formModel.RememberMe, false);

			if (!result.Succeeded) 
			{
				TempData[ErrorMessage] = "There was an error while logging you in! Please try again later or contact us.";
				return this.View(formModel);
			}

			if (formModel.ReturnUrl != null)
			{
				return this.RedirectToAction(formModel.ReturnUrl);
			}

			return this.RedirectToAction("Index", "Home");
		}
	}
}
