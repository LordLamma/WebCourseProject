using GameCatalogue.Data;
using GameCatalogue.Data.Models;
using GameCatalogue.Services.Data.Interfaces;
using GameCatalouge.Web.ViewModels.Discover;
using Microsoft.AspNetCore.Identity;

namespace GameCatalogue.Services.Data
{
    public class GameService : IGameService
    {
        private readonly GameCatalogueDbContext dbContext;

        public GameService(GameCatalogueDbContext dbContext, UserManager<ModdedUser> userManager)
        {
            this.dbContext = dbContext;
        }

        public Task<IEnumerable<DiscoverViewModel>> ThreeRandomGames()
        {
            throw new NotImplementedException();
        }
    }
}
