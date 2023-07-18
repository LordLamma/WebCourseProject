namespace GameCatalogue.Services.Data
{
    using GameCatalogue.Data;
    using GameCatalogue.Services.Data.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    internal class DeveloperService : IDeveloperService
    {
        private readonly GameCatalogueDbContext dbContext;

        public DeveloperService(GameCatalogueDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> DeveloperExistsByUserId(string userId)
        {
            bool result = await this.dbContext
                .Developers
                .AnyAsync(d => d.UserId.ToString() == userId);

            return result;
        }
    }
}
