namespace GameCatalogue.Services.Data
{
	using GameCatalogue.Data;
	using GameCatalogue.Data.Models;
	using GameCatalogue.Services.Data.Interfaces;
	using GameCatalogue.Services.Data.Models.Game;
    using GameCatalouge.Web.ViewModels.Developer;
	using GameCatalouge.Web.ViewModels.Game;
	using GameCatalouge.Web.ViewModels.Home;
	using GameCatalouge.Web.Views.Game.Enums;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using System.Data.SqlTypes;

	public class GameService : IGameService
    {
        private readonly GameCatalogueDbContext dbContext;

        public GameService(GameCatalogueDbContext dbContext, UserManager<ModdedUser> userManager)
        {
            this.dbContext = dbContext;
        }

		public async Task<AllGamesFilterServiceModel> AllAsync(AllGamesQueryModel queryModel)
		{
			IQueryable<Game> gamesQuery = this.dbContext
                .Games
                .Where(g => g.IsDeleted == false)
				.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryModel.Genre))
            {
                gamesQuery = gamesQuery
                    .Where(g => g.Genre.Name == queryModel.Genre);
            }
            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";
                gamesQuery = gamesQuery
                    .Where(g => EF.Functions.Like(g.Name, wildCard));
            }

            gamesQuery = queryModel.GameSorting switch
            {
                GameSorting.Alphabetical => gamesQuery
                    .OrderBy(g => g.Name),
                GameSorting.Newest => gamesQuery
                    .OrderByDescending(g => g.CreatedOn),
                GameSorting.Oldest => gamesQuery
                    .OrderBy(g => g.CreatedOn),
                _ => gamesQuery
                    .OrderByDescending(g => g.CreatedOn)
            };

            IEnumerable<GameAllViewModel> pagedGames = await gamesQuery
				.Skip((queryModel.CurrentPage - 1) * queryModel.GamesPerPage)
                .Take(queryModel.GamesPerPage)
                .Select(g => new GameAllViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    ImageURL = g.ImageURL,
                    Genre = g.Genre.Name
                })
                .ToArrayAsync();

            int totalGames = gamesQuery.Count();

            return new AllGamesFilterServiceModel()
            {
                TotalGamesCount = totalGames,
                Games = pagedGames
            };
		}

		public async Task<IEnumerable<GameAllViewModel>> AllByDeveloperIdAsync(string developerId)
		{
            IEnumerable<GameAllViewModel> allDeveloperGames = await this.dbContext
                .Games
                .Where(g => g.IsDeleted == false)
                .Where(g => g.DeveloperId.ToString() == developerId)
                .Select(g => new GameAllViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    ImageURL = g.ImageURL,
                    Genre = g.Genre.Name
                })
                .ToArrayAsync();

            return allDeveloperGames;
		}

		public async Task<string> Create(GameFormModel formModel, string developerId)
		{
            Game newGame = new Game()
            {
                Name = formModel.Name,
                Description = formModel.Description,
                ImageURL = formModel.ImageURL,
                GenreId = formModel.GenreId,
                DeveloperId = Guid.Parse(developerId)
            };

            dbContext.Games.Add(newGame);
            await dbContext.SaveChangesAsync();

            return newGame.Id.ToString();
		}

		public async Task DeleteGameByIdAsync(string gameId)
		{
            Game game = await this.dbContext
                .Games
                .Where(g => g.IsDeleted == false)
                .FirstAsync(g => g.Id.ToString() == gameId);

            game.IsDeleted = true;

            await this.dbContext.SaveChangesAsync();
		}

		public async Task EditGameByIdAndFormModel(string gameId, GameFormModel formModel)
		{
            Game game = await this.dbContext
                .Games
                .Where(g => g.IsDeleted == false)
                .FirstAsync(g => g.Id.ToString() == gameId);

            game.Name = formModel.Name;
            game.Description = formModel.Description;
            game.ImageURL = formModel.ImageURL;
            game.GenreId = formModel.GenreId;

            await this.dbContext.SaveChangesAsync();
		}

		public async Task<bool> ExistsByIdAsync(string gameId)
		{
            bool exists = await this.dbContext
                .Games
                .AnyAsync(g => g.Id.ToString() == gameId);

            return exists;
		}

		public async Task<GameDetailsViewModel> GetDetailsByIdAsync(string gameId)
		{
            Game game = await this.dbContext
                .Games
                .Include(g => g.Genre)
                .Include(g => g.Developer)
                .ThenInclude(d => d.User)
				.Where(g => g.IsDeleted == false)
				.FirstAsync(g => g.Id.ToString() == gameId);

            return new GameDetailsViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                ImageURL = game.ImageURL,
                Genre = game.Genre.Name,
                Developer = new DeveloperDetailsInfoViewModel()
                {
                    DisplayName = game.Developer.User.UserName,
                    BusinessEmail = game.Developer.BusinessEmail
                }
            };
		}

		public async Task<GamePreDeleteDetailsViewModel> GetGameDetailsForDeleteByIdAsync(string gameId)
		{
            Game game = await this.dbContext
                .Games
				.Where(g => g.IsDeleted == false)
				.FirstAsync(g => g.Id.ToString() == gameId);

            return new GamePreDeleteDetailsViewModel()
            {
                Name = game.Name,
                Description = game.Description,
                ImageURL = game.ImageURL
            };
		}

		public async Task<GameFormModel> GetGameForEditByIdAsync(string gameId)
		{
			Game game = await this.dbContext
				.Games
				.Include(g => g.Genre)
				.Where(g => g.IsDeleted == false)
				.FirstAsync(g => g.Id.ToString() == gameId);

            return new GameFormModel
            {
                Name = game.Name,
                Description = game.Description,
                ImageURL = game.ImageURL,
                GenreId = game.Genre.Id
            };
		}

		public async Task<bool> IsDeveloperByIdProducerOfGameByIdAsync(string gameId, string developerId)
		{
            Game game = await this.dbContext
                .Games
				.Where(g => g.IsDeleted == false)
				.FirstAsync(g => g.Id.ToString() == gameId);

            return game.DeveloperId.ToString() == developerId;
		}

		public async Task<IEnumerable<IndexViewModel>> LastThreeGames()
        {
            IEnumerable<IndexViewModel> lastThreeGames = await this.dbContext
                .Games
                .OrderByDescending(g => g.CreatedOn)
                .Take(3)
                .Select(g=> new IndexViewModel()
                { 
                    Id = g.Id.ToString(),
                    Name = g.Name,
                    ImageURL = g.ImageURL
                })
                .ToArrayAsync();
            return lastThreeGames;
        }

        public async Task<IEnumerable<GameAllViewModel>> ThreeRandomGames(string userId)
        {
            IEnumerable<GameAllViewModel> threeRandomGames = await this.dbContext
                .Games
                .Include(g => g.Developer)
                .Where(g => g.Developer.UserId.ToString() != userId)
                .OrderBy(g => Guid.NewGuid())
                .Take(3)
                .Select (g=> new GameAllViewModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    ImageURL = g.ImageURL,
                    Genre = g.Genre.Name
                })
                .ToListAsync();

            return threeRandomGames;
        }
    }
}
