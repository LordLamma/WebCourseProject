using GameCatalogue.Data;
using GameCatalogue.Data.Models;
using GameCatalogue.Services.Data.Interfaces;
using GameCatalogue.Services.Data.Models.Game;
using GameCatalouge.Web.ViewModels.Discover;
using GameCatalouge.Web.ViewModels.Game;
using GameCatalouge.Web.ViewModels.Home;
using GameCatalouge.Web.Views.Game.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameCatalogue.Services.Data
{
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

		public async Task Create(GameFormModel formModel, string developerId)
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

        public Task<IEnumerable<DiscoverViewModel>> ThreeRandomGames()
        {
            throw new NotImplementedException();
        }
    }
}
