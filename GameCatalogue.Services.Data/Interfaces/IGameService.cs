namespace GameCatalogue.Services.Data.Interfaces
{
	using GameCatalogue.Services.Data.Models.Game;
	using GameCatalouge.Web.ViewModels.Discover;
	using GameCatalouge.Web.ViewModels.Game;
	using GameCatalouge.Web.ViewModels.Home;

    public interface IGameService
    {
        Task<IEnumerable<IndexViewModel>> LastThreeGames();
        Task<IEnumerable<DiscoverViewModel>> ThreeRandomGames();
        Task Create(GameFormModel formModel, string developerId);

        Task<AllGamesFilterServiceModel> AllAsync(AllGamesQueryModel queryModel);
    }
}
