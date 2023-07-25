namespace GameCatalouge.Web.Controllers
{
	using GameCatalogue.Services.Data.Interfaces;
	using GameCatalogue.Services.Data.Models.Game;
	using GameCatalogue.Web.Infrastructure;
	using GameCatalouge.Web.ViewModels.Game;
	using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
	using System.Reflection.Metadata.Ecma335;
	using static GameCatalogue.Common.NotificationMessages;

    [Authorize]
    public class GameController : Controller
    {
        private readonly IGenreService genreService;
        private readonly IDeveloperService developerService;
        private readonly IGameService gameService;

        public GameController(IGenreService genreService, IDeveloperService developerService, IGameService gameService)
        {
            this.genreService = genreService;
            this.developerService = developerService;
            this.gameService = gameService;
		}

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]AllGamesQueryModel queryModel)
        {
            AllGamesFilterServiceModel serviceModel = await this.gameService.AllAsync(queryModel);

            queryModel.Games = serviceModel.Games;
            queryModel.TotalGames = serviceModel.TotalGamesCount;
            queryModel.Genres = await this.genreService.AllGenreNamesAsync();

            return this.View(queryModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            bool isDeveloper = await this.developerService.DeveloperExistsByUserId(this.User.GetId()!);

            if (!isDeveloper)
            {
                this.TempData[ErrorMessage] = "You must be a developer to add new games!";
                return RedirectToAction("Become", "Developer");
            }

            GameFormModel formModel = new GameFormModel()
            {
                Genres = await this.genreService.GetAllGenresAsync()
            };
            return View(formModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(GameFormModel formModel)
        {
			bool isDeveloper = await this.developerService.DeveloperExistsByUserId(this.User.GetId()!);

			if (!isDeveloper)
			{
				this.TempData[ErrorMessage] = "You must be a developer to add new games!";
				return RedirectToAction("Become", "Developer");
			}

            bool categoryExists = await this.genreService.ExistsById(formModel.GenreId);

            if (!categoryExists)
            {
				this.ModelState.AddModelError(nameof(formModel.GenreId), "Selected category does not exist");
            }

            if (!this.ModelState.IsValid)
            {
                formModel.Genres = await this.genreService.GetAllGenresAsync();

                return this.View(formModel);
			}

            try
            {
                string? developerId = await this.developerService.DeveloperIdByUserId(this.User.GetId()!);
                await this.gameService.Create(formModel, developerId!);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, "An unexpected error occured while trying to add the game! Please try again later or contact us!");
				formModel.Genres = await this.genreService.GetAllGenresAsync();
				return this.View(formModel);
            }

            return this.RedirectToAction("All", "Game");
		}
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            GameDetailsViewModel? viewModel = await this.gameService.GetDetailsByIdAsync(id);
            if (viewModel == null)
            {
                this.TempData[ErrorMessage] = "A game with the provided id does not exist!";
                this.RedirectToAction("All", "Game");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            List<GameAllViewModel> myGames = new List<GameAllViewModel>();

            string userId = this.User.GetId()!;

            string developerId = await this.developerService.DeveloperIdByUserId(userId);

            myGames.AddRange(await this.gameService.AllByDeveloperIdAsync(developerId!));

            return this.View(myGames);
        }
    }
}
