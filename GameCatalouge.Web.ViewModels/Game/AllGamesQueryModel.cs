namespace GameCatalouge.Web.ViewModels.Game
{
	using GameCatalouge.Web.Views.Game.Enums;
	using System.ComponentModel.DataAnnotations;
	using static GameCatalogue.Common.GeneralConstants;

	public class AllGamesQueryModel
	{
        public AllGamesQueryModel()
        {
			this.CurrentPage = StartingPage;
			this.GamesPerPage = AllowedGamesPerPage;

			this.Genres = new HashSet<string>();
			this.Games = new HashSet<GameAllViewModel>();
        }

        public string? Genre { get; set; }

		[Display(Name = "Search by a keyword")]
		public string? SearchString { get; set; }

		[Display(Name = "Sort by")]
		public GameSorting GameSorting { get; set; }

		public int CurrentPage { get; set; }

		[Display(Name = "How many games on page")]
		public int GamesPerPage { get; set; }

		public int TotalGames { get; set; }

		public IEnumerable<string> Genres { get; set; }

		public IEnumerable<GameAllViewModel> Games { get; set; }
	}
}
