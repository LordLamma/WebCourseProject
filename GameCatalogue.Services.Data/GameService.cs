using GameCatalogue.Data;
using GameCatalogue.Data.Models;
using GameCatalogue.Services.Data.Interfaces;
using GameCatalouge.Web.ViewModels.Discover;
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
