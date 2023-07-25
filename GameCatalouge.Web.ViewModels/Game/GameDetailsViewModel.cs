namespace GameCatalouge.Web.ViewModels.Game
{
	using GameCatalouge.Web.ViewModels.Developer;

	public class GameDetailsViewModel : GameAllViewModel
	{
		public string Description { get; set; } = null!;

		public DeveloperDetailsInfoViewModel Developer { get; set; } = null!;
	}
}
