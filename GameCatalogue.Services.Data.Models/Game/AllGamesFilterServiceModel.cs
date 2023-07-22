namespace GameCatalogue.Services.Data.Models.Game
{
	using GameCatalouge.Web.ViewModels.Game;

	public class AllGamesFilterServiceModel
	{
        public AllGamesFilterServiceModel()
        {
            this.Games = new HashSet<GameAllViewModel>();
        }

        public int TotalGamesCount { get; set; }

		public IEnumerable<GameAllViewModel> Games { get; set; }
	}
}
