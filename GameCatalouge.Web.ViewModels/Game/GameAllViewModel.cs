
namespace GameCatalouge.Web.ViewModels.Game
{
	using System.ComponentModel.DataAnnotations;

	public class GameAllViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		[Display(Name = "Image link")]
		public string ImageURL { get; set; } = null!;

		[Display(Name = "Genre")]
		public string Genre { get; set; } = null!;
	}
}
