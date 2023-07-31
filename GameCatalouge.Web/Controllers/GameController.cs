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

            try
            {
				GameFormModel formModel = new GameFormModel()
				{
					Genres = await this.genreService.GetAllGenresAsync()
				};
				return View(formModel);
			}
            catch (Exception)
			{
				return this.GeneralError();
			}
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
                string gameId = 
                    await this.gameService.Create(formModel, developerId!);
                this.TempData[SuccessMessage] = "The game was published successfully!";

				return this.RedirectToAction("Details", "Game", new { id = gameId });
			}
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, "An unexpected error occured while trying to add the game! Please try again later or contact us!");
				formModel.Genres = await this.genreService.GetAllGenresAsync();
				return this.View(formModel);
            }

		}
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            bool gameExists = await this.gameService
                .ExistsByIdAsync(id);

            if (!gameExists)
            {
				this.TempData[ErrorMessage] = "A game with the provided id does not exist!";
				return this.RedirectToAction("All", "Game");
			}

            try
            {
				GameDetailsViewModel viewModel = await this.gameService.GetDetailsByIdAsync(id);

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
			bool gameExists = await this.gameService
				.ExistsByIdAsync(id);

			if (!gameExists)
			{
				this.TempData[ErrorMessage] = "A game with the provided id does not exist!";
				return this.RedirectToAction("All", "Game");
			}

            bool isUserDeveloper = await this.developerService
                .DeveloperExistsByUserId(this.User.GetId());
            if (!isUserDeveloper && !this.User.IsAdmin()) 
            {
                this.TempData[ErrorMessage] = "You must become a developer to edit game info";
                return this.RedirectToAction("Become", "Developer");
            }

            string? devId = await this.developerService.DeveloperIdByUserId(this.User.GetId());
            bool isDeveloperProducer = await this.gameService
                .IsDeveloperByIdProducerOfGameByIdAsync(id, devId!);
            if (!isDeveloperProducer && !this.User.IsAdmin())
            {
                this.TempData[ErrorMessage] = "You can only edit games you have published!";
                return this.RedirectToAction("Mine", "Game");
            }

            try
            {
				GameFormModel formModel = await this.gameService
				.GetGameForEditByIdAsync(id);
				formModel.Genres = await this.genreService.GetAllGenresAsync();
				return this.View(formModel);
			}
            catch (Exception)
            {
                return this.GeneralError();
			}
		}

        [HttpPost]
        public async Task<IActionResult> Edit(string id,  GameFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                formModel.Genres = await this.genreService.GetAllGenresAsync();
                return this.View(formModel);
            }

			bool gameExists = await this.gameService
				.ExistsByIdAsync(id);

			if (!gameExists)
			{
				this.TempData[ErrorMessage] = "A game with the provided id does not exist!";
				return this.RedirectToAction("All", "Game");
			}

			bool isUserDeveloper = await this.developerService
				.DeveloperExistsByUserId(this.User.GetId());
			if (!isUserDeveloper && !this.User.IsAdmin())
			{
				this.TempData[ErrorMessage] = "You must become a developer to edit game info";
				return this.RedirectToAction("Become", "Developer");
			}

			string? devId = await this.developerService.DeveloperIdByUserId(this.User.GetId());
			bool isDeveloperProducer = await this.gameService
				.IsDeveloperByIdProducerOfGameByIdAsync(id, devId!);
			if (!isDeveloperProducer && !this.User.IsAdmin())
			{
				this.TempData[ErrorMessage] = "You can only edit games you have published!";
				return this.RedirectToAction("Mine", "Game");
			}

            try
            {
                await this.gameService.EditGameByIdAndFormModel(id, formModel);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(String.Empty, "An unexpected error occured while trying to save your changes. Please try again later or contact an administrator!");
                formModel.Genres = await this.genreService.GetAllGenresAsync();
                return this.View(formModel);
            }
			this.TempData[SuccessMessage] = "Changes saved successfully!";
			return this.RedirectToAction("Details", "Game", new { id });
		}

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
			bool gameExists = await this.gameService
				.ExistsByIdAsync(id);

			if (!gameExists)
			{
				this.TempData[ErrorMessage] = "A game with the provided id does not exist!";
				return this.RedirectToAction("All", "Game");
			}

			bool isUserDeveloper = await this.developerService
				.DeveloperExistsByUserId(this.User.GetId());
			if (!isUserDeveloper && !this.User.IsAdmin())
			{
				this.TempData[ErrorMessage] = "You must become a developer to delete games";
				return this.RedirectToAction("Become", "Developer");
			}

			string? devId = await this.developerService.DeveloperIdByUserId(this.User.GetId());
			bool isDeveloperProducer = await this.gameService
				.IsDeveloperByIdProducerOfGameByIdAsync(id, devId!);
			if (!isDeveloperProducer && !this.User.IsAdmin())
			{
				this.TempData[ErrorMessage] = "You can only delete games you have published!";
				return this.RedirectToAction("Mine", "Game");
			}

            try
            {
                GamePreDeleteDetailsViewModel viewModel = 
                    await this.gameService.GetGameDetailsForDeleteByIdAsync(id);

                return this.View(viewModel);
            }
            catch (Exception)
            {
                return this.GeneralError();
            }
		}

        [HttpPost]
        public async Task<IActionResult> Delete(string id, GamePreDeleteDetailsViewModel viewModel)
        {
			bool gameExists = await this.gameService
				.ExistsByIdAsync(id);

			if (!gameExists)
			{
				this.TempData[ErrorMessage] = "A game with the provided id does not exist!";
				return this.RedirectToAction("All", "Game");
			}

			bool isUserDeveloper = await this.developerService
				.DeveloperExistsByUserId(this.User.GetId());
			if (!isUserDeveloper && !this.User.IsAdmin())
			{
				this.TempData[ErrorMessage] = "You must become a developer to delete games";
				return this.RedirectToAction("Become", "Developer");
			}

			string? devId = await this.developerService.DeveloperIdByUserId(this.User.GetId());
			bool isDeveloperProducer = await this.gameService
				.IsDeveloperByIdProducerOfGameByIdAsync(id, devId!);
			if (!isDeveloperProducer && !this.User.IsAdmin())
			{
				this.TempData[ErrorMessage] = "You can only delete games you have published!";
				return this.RedirectToAction("Mine", "Game");
			}

            try
            {
                await this.gameService.DeleteGameByIdAsync(id);

                this.TempData[WarningMessage] = "The game has successfully been deleted!";
                return this.RedirectToAction("Mine", "Game");
            }
            catch (Exception)
            {
                return this.GeneralError();
            }
		}

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            List<GameAllViewModel> myGames = new List<GameAllViewModel>();

            string userId = this.User.GetId()!;

            try
            {
				string developerId = await this.developerService.DeveloperIdByUserId(userId);

				myGames.AddRange(await this.gameService.AllByDeveloperIdAsync(developerId!));

				return this.View(myGames);
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
