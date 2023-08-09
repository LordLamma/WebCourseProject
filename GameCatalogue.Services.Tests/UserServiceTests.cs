namespace GameCatalogue.Services.Tests
{
    using GameCatalogue.Data;
    using GameCatalogue.Services.Data.Interfaces;
    using GameCatalogue.Services.Data;
    using Microsoft.EntityFrameworkCore;
    using static GameCatalogue.Services.Tests.DatabaseSeeder;
    using GameCatalouge.Web.ViewModels.User;

    internal class UserServiceTests
    {
        private DbContextOptions<GameCatalogueDbContext> dbOptions;
        private GameCatalogueDbContext dbContext;

        private IUserService userService;

        [SetUp]
        public void SetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<GameCatalogueDbContext>()
                .UseInMemoryDatabase("GameCatalogueInMemory" + Guid.NewGuid().ToString())
                .Options;
            this.dbContext = new GameCatalogueDbContext(this.dbOptions);

            dbContext.Database.EnsureCreated();

            SeedDatabase(this.dbContext);

            this.userService = new UserService(this.dbContext);
        }

        [Test]
        public async Task AllAsyncShouldReturnAListOfAllUsersWithDataInside()
        {
            IEnumerable<AllUserViewModel> allUsers = await this.userService.AllAsync();

            Assert.That(allUsers.Count,Is.EqualTo(this.dbContext.Users.Count()));

            foreach (var user in allUsers)
            {
                Assert.IsNotNull(user.UserName);
                Assert.IsNotNull(user.Email);
            }
        }
    }
}
