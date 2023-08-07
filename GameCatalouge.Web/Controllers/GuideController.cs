namespace GameCatalouge.Web.Controllers
{
    using GameCatalouge.Web.ViewModels.Guide;
    using GameCatalogue.Services.Data.Models.Guide;
    using GameCatalogue.Services.Data.Interfaces;
    using GameCatalogue.Web.Infrastructure;
    using static GameCatalogue.Common.NotificationMessages;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using GameCatalouge.Web.ViewModels.Game;

    [Authorize]
    public class GuideController : Controller
    {
        private readonly IGuideService guideService;
        public GuideController(IGuideService guideService)
        {
            this.guideService = guideService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> All([FromQuery] GuideAllQueryModel queryModel)
        {
            AllGuidesFilterServiceModel serviceModel = await this.guideService.AllAsync(queryModel);

            queryModel.Guides = serviceModel.Guides;
            queryModel.TotalGuides = serviceModel.TotalGuidesCount;
            
            return this.View(queryModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(GuideFormModel formModel)
        {
            try
            {
                string? authorId = this.User.GetId();
                string gameId =
                    await this.guideService.Create(formModel, authorId!);
                this.TempData[SuccessMessage] = "The guide was added successfully!";

                return this.RedirectToAction("Details", "Guide", new { id = gameId });
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, "An unexpected error occured while trying to add the game! Please try again later or contact us!");
                return this.View(formModel);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            bool gameExists = await this.guideService
                .ExistsByIdAsync(id);

            if (!gameExists)
            {
                this.TempData[ErrorMessage] = "A game with the provided id does not exist!";
                return this.RedirectToAction("All", "Game");
            }

            try
            {
                GuideDetailsViewModel viewModel = await this.guideService.GetDetailsByIdAsync(id);

                return View(viewModel);
            }
            catch (Exception)
            {
                return this.GeneralError();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            bool guideExists = await this.guideService
                .ExistsByIdAsync(id);

            if (!guideExists)
            {
                this.TempData[ErrorMessage] = "A guide with the provided id does not exist!";
                return this.RedirectToAction("All", "Guide");
            }

            string? userId = this.User.GetId();
            bool isUserAuthor = await this.guideService
                .IsUserByIdWriterOfGuideById(id, userId);
            if (!isUserAuthor && !this.User.IsAdmin())
            {
                this.TempData[ErrorMessage] = "You can only edit guides you have writen!";
                return this.RedirectToAction("All", "Guide");
            }

            try
            {
                GuideFormModel formModel = await this.guideService
                .GetGuideForEditByIdAsync(id);
                return this.View(formModel);
            }
            catch (Exception)
            {
                return this.GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, GuideFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            bool guideExists = await this.guideService
                .ExistsByIdAsync(id);

            if (!guideExists)
            {
                this.TempData[ErrorMessage] = "A guide with the provided id does not exist!";
                return this.RedirectToAction("All", "Guide");
            }

            string? userId = this.User.GetId();
            bool isUserAuthor = await this.guideService
                .IsUserByIdWriterOfGuideById(id, userId);
            if (!isUserAuthor && !this.User.IsAdmin())
            {
                this.TempData[ErrorMessage] = "You can only edit guides you have writen!";
                return this.RedirectToAction("All", "Guide");
            }

            try
            {
                await this.guideService.EditGuideByIdAndFormModel(id, formModel);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(String.Empty, "An unexpected error occured while trying to save your changes. Please try again later or contact an administrator!");
                return this.View(formModel);
            }
            this.TempData[SuccessMessage] = "Changes saved successfully!";
            return this.RedirectToAction("Details", "Guide", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool guideExists = await this.guideService
                .ExistsByIdAsync(id);

            if (!guideExists)
            {
                this.TempData[ErrorMessage] = "A guide with the provided id does not exist!";
                return this.RedirectToAction("All", "Guide");
            }

            string? userId = this.User.GetId();
            bool isUserAuthor = await this.guideService
                .IsUserByIdWriterOfGuideById(id, userId);
            if (!isUserAuthor && !this.User.IsAdmin())
            {
                this.TempData[ErrorMessage] = "You can only delete guides you have writen!";
                return this.RedirectToAction("All", "Guide");
            }

            try
            {
                GuidePreDeleteViewModel viewModel =
                    await this.guideService.GetGuideDetailsForDeleteByIdAsync(id);

                return this.View(viewModel);
            }
            catch (Exception)
            {
                return this.GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, GuidePreDeleteViewModel viewModel)
        {
            bool guideExists = await this.guideService
                .ExistsByIdAsync(id);

            if (!guideExists)
            {
                this.TempData[ErrorMessage] = "A guide with the provided id does not exist!";
                return this.RedirectToAction("All", "Guide");
            }

            string? userId = this.User.GetId();
            bool isUserAuthor = await this.guideService
                .IsUserByIdWriterOfGuideById(id, userId);
            if (!isUserAuthor && !this.User.IsAdmin())
            {
                this.TempData[ErrorMessage] = "You can only delete guides you have writen!";
                return this.RedirectToAction("All", "Guide");
            }

            try
            {
                await this.guideService.DeleteGuideByIdAsync(id);

                this.TempData[WarningMessage] = "The guide has successfully been deleted!";
                return this.RedirectToAction("All", "Guide");
            }
            catch (Exception)
            {
                return this.GeneralError();
            }
        }

        private IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] = "An unexpected error occured! Please try again later or contact an administrator";
            return this.RedirectToAction("Index", "Home");
        }
    }
}
