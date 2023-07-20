namespace GameCatalouge.Web.ViewModels.Game
{
	using GameCatalouge.Web.ViewModels.Genre;
	using System.ComponentModel.DataAnnotations;
	using static GameCatalogue.Common.EntityValidationConstants.Game;

	public class GameFormModel
	{
        public GameFormModel()
        {
            this.Genres = new HashSet<SelectGenreFormModel>();
        }

		[Required]
		[StringLength(NameMaxLenght,MinimumLength = NameMinLenght)]
        public string Name { get; set; } = null!;

		[Required]
		[StringLength(DescriptionMaxLenght, MinimumLength = DescriptionMinLength)]
		public string Description { get; set; } = null!;

		[Required]
		[StringLength(ImageURLMaxLength)]
		[Display(Name = "Image Link")]
		public string ImageURL { get; set; } = null!;

		[Display(Name = "Genre")]
		public int GenreId { get;set; }

		public IEnumerable<SelectGenreFormModel> Genres { get; set; }
	}
}
