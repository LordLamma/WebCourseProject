namespace GameCatalogue.Services.Tests
{
    using GameCatalogue.Data;
    using GameCatalogue.Services.Data.Interfaces;
    using GameCatalogue.Services.Data;
    using GameCatalouge.Web.ViewModels.Genre;
    using static GameCatalogue.Services.Tests.DatabaseSeeder;

    using Microsoft.EntityFrameworkCore;

    internal class GenreServiceTests
    {
        private DbContextOptions<GameCatalogueDbContext> dbOptions;
        private GameCatalogueDbContext dbContext;

        private IGenreService genreService;

        [SetUp]
        public void SetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<GameCatalogueDbContext>()
                .UseInMemoryDatabase("GameCatalogueInMemory" + Guid.NewGuid().ToString())
                .Options;
            this.dbContext = new GameCatalogueDbContext(this.dbOptions);

            dbContext.Database.EnsureCreated();

            SeedDatabase(this.dbContext);

            this.genreService = new GenreService(this.dbContext);
        }

        [Test]
        public async Task AllGenreNamesAsyncShouldReturnACollectionWithAllGenreNames()
        {
            IEnumerable<string> names = await this.genreService.AllGenreNamesAsync();

            foreach (var name in names)
            {
                Assert.IsNotNull(name);
            }
        }

        [Test]
        public async Task ExistsByIdShouldReturnTrueWhenGivenExistingId()
        {
            int existingId = GenreOne.Id;

            bool result = await this.genreService.ExistsById(existingId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task ExistsByIdShouldReturnFalseWhenGivenNonexistingId()
        {
            int nonExistingId = -1;

            bool result = await this.genreService.ExistsById(nonExistingId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllGenresAsyncShouldReturnACollectionWithAllGenres()
        {
            IEnumerable<SelectGenreFormModel> genres = await this.genreService.GetAllGenresAsync();

            foreach (var genre in genres)
            {
                Assert.IsNotNull(genre.Id);
                Assert.IsNotNull(genre.Name);
            }
        }
    }
}
