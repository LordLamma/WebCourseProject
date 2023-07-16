namespace GameCatalogue.Services.Data.Interfaces
{
    using GameCatalouge.Web.ViewModels.Discover;
    using GameCatalouge.Web.ViewModels.Home;

    public interface IGameService
    {
        Task<IEnumerable<IndexViewModel>> LastThreeGames();
        Task<IEnumerable<DiscoverViewModel>> ThreeRandomGames();
    }
}
