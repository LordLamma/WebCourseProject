namespace GameCatalogue.Services.Data
{
    using GameCatalogue.Data;
	using GameCatalogue.Data.Models;
	using GameCatalogue.Services.Data.Interfaces;
	using GameCatalouge.Web.ViewModels.Developer;
	using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    internal class DeveloperService : IDeveloperService
    {
        private readonly GameCatalogueDbContext dbContext;

        public DeveloperService(GameCatalogueDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task Create(string userId, BecomeDeveloperFormModel model)
		{
			Developer developer = new Developer()
			{
				BusinessEmail = model.BusinessEmail,
				UserId = Guid.Parse(userId)
			};

			await this.dbContext.Developers.AddAsync(developer);
			await this.dbContext.SaveChangesAsync();
		}

		public async Task<bool> DeveloperExistsByEmail(string email)
		{
			bool result = await this.dbContext
				.Developers
				.AnyAsync(d => d.BusinessEmail == email);

			return result;
		}

		public async Task<bool> DeveloperExistsByUserId(string userId)
        {
            bool result = await this.dbContext
                .Developers
                .AnyAsync(d => d.UserId.ToString() == userId);

            return result;
        }

		public async Task<string?> DeveloperIdByUserId(string userId)
		{
			Developer? developer = await this.dbContext
				.Developers
				.FirstOrDefaultAsync(d => d.UserId.ToString() == userId);

			if (developer == null)
			{
				return null;
			}

			return developer.Id.ToString();
		}
	}
}
