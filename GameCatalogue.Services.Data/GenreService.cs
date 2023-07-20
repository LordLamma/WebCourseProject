namespace GameCatalogue.Services.Data
{
	using GameCatalogue.Data;
	using GameCatalogue.Services.Data.Interfaces;
	using GameCatalouge.Web.ViewModels.Genre;
	using Microsoft.EntityFrameworkCore;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class GenreService : IGenreService
	{
		private readonly GameCatalogueDbContext dbContext;

        public GenreService(GameCatalogueDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task<bool> ExistsById(int id)
		{
			bool result = await this.dbContext
				.Genres
				.AnyAsync(g => g.Id == id);

			return result;
		}

		public async Task<IEnumerable<SelectGenreFormModel>> GetAllGenresAsync()
		{
			IEnumerable<SelectGenreFormModel> allGenres = await this.dbContext
				.Genres
				.AsNoTracking()
				.Select(g => new SelectGenreFormModel()
				{
					Id = g.Id,
					Name = g.Name
				})
				.ToArrayAsync();

			return allGenres;
		}
	}
}
