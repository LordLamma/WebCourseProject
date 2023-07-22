namespace GameCatalogue.Services.Data.Interfaces
{
	using GameCatalouge.Web.ViewModels.Genre;

	public interface IGenreService
	{
		Task<IEnumerable<SelectGenreFormModel>> GetAllGenresAsync();
		Task<bool> ExistsById(int id);

		Task<IEnumerable<string>> AllGenreNamesAsync();
	}
}
