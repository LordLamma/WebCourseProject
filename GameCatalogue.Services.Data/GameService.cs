using GameCatalogue.Data;
using GameCatalogue.Data.Models;
using GameCatalogue.Services.Data.Interfaces;
using GameCatalouge.Web.ViewModels.Discover;
using GameCatalouge.Web.ViewModels.Game;
using GameCatalouge.Web.ViewModels.Home;
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
