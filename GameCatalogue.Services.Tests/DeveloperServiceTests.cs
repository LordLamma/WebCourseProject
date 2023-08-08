namespace GameCatalogue.Services.Tests
{
    using GameCatalogue.Data;

    using GameCatalogue.Services.Data;
    using GameCatalogue.Services.Data.Interfaces;
    using GameCatalouge.Web.ViewModels.Developer;

    using static GameCatalogue.Services.Tests.DatabaseSeeder;

    using Microsoft.EntityFrameworkCore;

    public class DeveloperServiceTests
    {
        private DbContextOptions<GameCatalogueDbContext> dbOptions;
        private GameCatalogueDbContext dbContext;

        private IDeveloperService developerService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<GameCatalogueDbContext>()
                .UseInMemoryDatabase("GameCatalogueInMemory" + Guid.NewGuid().ToString())
                .Options;
            this.dbContext = new GameCatalogueDbContext(this.dbOptions);

            dbContext.Database.EnsureCreated();

            SeedDatabase(this.dbContext);

            this.developerService = new DeveloperService(this.dbContext);
        }

        [Test]
        public async Task DeveloperExistsByUserIdShouldReturnTrueWhenTheDeveloperExists()
        {
            string existingDeveloperUserId = DeveloperUser.Id.ToString();

            bool result = await this.developerService.DeveloperExistsByUserId(existingDeveloperUserId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeveloperExistsByUserIdShouldReturnFalseWhenTheDeveloperDoesntExist()
        {
            string nonExistingDeveloperUserId = RegularUser.Id.ToString();

            bool result = await this.developerService.DeveloperExistsByUserId(nonExistingDeveloperUserId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeveloperExistsByEmailShouldReturnTrueWhenTheDeveloperExists()
        {
            string existingDeveloperEmail = Developer.BusinessEmail;

            bool result = await this.developerService.DeveloperExistsByEmail(existingDeveloperEmail);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeveloperExistsByEmailShouldReturnFalseWhenTheDeveloperDoesntExist()
        {
            string nonExistingDeveloperEmail = RegularUser.Email;

            bool result = await this.developerService.DeveloperExistsByUserId(nonExistingDeveloperEmail);

            Assert.IsFalse(result);
        }


        [Test]
        public async Task DeveloperIdByUserIdShouldReturnTrueWhenPresentedWithCorrectId()
        {
            string DeveloperId = Developer.Id.ToString();
            string UserId = Developer.User.Id.ToString();

            string returnedDeveloperId = await this.developerService.DeveloperIdByUserId(UserId);

            bool result = DeveloperId == returnedDeveloperId ? true : false;

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeveloperIdByUserIdShouldReturnFalseWhenPresentedWithIncorrectId()
        {
            string DeveloperId = RegularUser.Id.ToString();
            string UserId = Developer.User.Id.ToString();

            string returnedDeveloperId = await this.developerService.DeveloperIdByUserId(UserId);

            bool result = DeveloperId == returnedDeveloperId ? true : false;

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeveloperIdByUserIdShouldReturnNullWhenPresentedWithNonExistentId()
        {
            string nonExistentId = "";

            string returnedDeveloperId = await this.developerService.DeveloperIdByUserId(nonExistentId);

            Assert.IsNull(returnedDeveloperId);
        }

        [Test]
        public async Task CreateShouldAddDeveloperToTheDatabaseWithInformationInside()
        {
            int developersBefore = this.dbContext.Developers.Count();

            BecomeDeveloperFormModel model = new BecomeDeveloperFormModel()
            {
                BusinessEmail = Developer.BusinessEmail
            };

            await developerService.Create(Developer.User.Id.ToString(), model);

            int developersAfter = this.dbContext.Developers.Count();

            Assert.That(developersAfter, Is.EqualTo(developersBefore + 1));

            string newDevId = await this.developerService.DeveloperIdByUserId(Developer.User.Id.ToString());
            var newDev = await this.dbContext.Developers.FindAsync(Guid.Parse(newDevId));

            Assert.IsNotNull(newDevId);
            Assert.That(newDev.UserId, Is.EqualTo(Developer.UserId));
            Assert.That(newDev.BusinessEmail, Is.EqualTo(Developer.BusinessEmail));
        }

        [Test]

        public async Task HasGameWithIdShouldReturnTrueIfTheDeveloperHasAGameWithTheId()
        {
            int gameId = Game.Id;
            string developerUserId = Developer.UserId.ToString();

            bool result = await this.developerService.HasGameWithId(developerUserId, gameId);

            Assert.IsTrue(result);
        }

        [Test]

        public async Task HasGameWithIdShouldReturnFalseIfTheDeveloperDoesntHaveAGameWithTheId()
        {
            int gameId = 2;
            string developerUserId = Developer.UserId.ToString();

            bool result = await this.developerService.HasGameWithId(developerUserId, gameId);

            Assert.IsFalse(result);
        }

        [Test]

        public async Task HasGameWithIdShouldReturnNullIfTheDeveloperDoesntExist()
        {
            int gameId = 2;
            string developerUserId = " ";

            bool result = await this.developerService.HasGameWithId(developerUserId, gameId);

            Assert.IsFalse(result);
        }
    }
}