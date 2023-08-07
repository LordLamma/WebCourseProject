namespace GameCatalouge.Web.Controllers
{
    using GameCatalouge.Web.ViewModels.Guide;
    using GameCatalogue.Services.Data.Models.Guide;
    using GameCatalogue.Services.Data.Interfaces;
    using GameCatalogue.Web.Infrastructure;
    using static GameCatalogue.Common.NotificationMessages;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

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

                return this.RedirectToAction("All", "Guide");

                //return this.RedirectToAction("Details", "Guide", new { id = gameId });
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, "An unexpected error occured while trying to add the game! Please try again later or contact us!");
                return this.View(formModel);
            }
        }
    }
}
