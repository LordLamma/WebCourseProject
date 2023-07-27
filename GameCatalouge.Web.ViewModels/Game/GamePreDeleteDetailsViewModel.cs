namespace GameCatalouge.Web.ViewModels.Game
{
	using System.ComponentModel.DataAnnotations;

	public class GamePreDeleteDetailsViewModel
	{
		public string Name { get; set; } = null!;

		public string Description { get; set; } = null!;

		[Display(Name = "Image link")]
		public string ImageURL { get; set; } = null!;
	}
}
