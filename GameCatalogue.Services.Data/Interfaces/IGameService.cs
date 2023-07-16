namespace GameCatalogue.Services.Data.Interfaces
{
    using GameCatalouge.Web.ViewModels.Discover;

    public interface IGameService
    {
        Task<IEnumerable<DiscoverViewModel>> ThreeRandomGames();
    }
}
