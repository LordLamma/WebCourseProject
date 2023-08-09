namespace GameCatalogue.Services.Tests
{
    using GameCatalogue.Data;
    using GameCatalogue.Services.Data.Interfaces;
    using GameCatalogue.Services.Data;
    using GameCatalouge.Web.ViewModels.Guide;
    using GameCatalogue.Services.Data.Models.Guide;
    using static GameCatalogue.Services.Tests.DatabaseSeeder;

    using Microsoft.EntityFrameworkCore;
    using GameCatalouge.Web.ViewModels.Game;

    internal class GuideServiceTests
    {
        private DbContextOptions<GameCatalogueDbContext> dbOptions;
        private GameCatalogueDbContext dbContext;

        private IGuideService guideService;

        [SetUp]
        public void SetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<GameCatalogueDbContext>()
                .UseInMemoryDatabase("GameCatalogueInMemory" + Guid.NewGuid().ToString())
                .Options;
            this.dbContext = new GameCatalogueDbContext(this.dbOptions);

            dbContext.Database.EnsureCreated();

            SeedDatabase(this.dbContext);

            this.guideService = new GuideService(this.dbContext);
        }

        [Test]
        public async Task AllAsyncShouldReturnAllGuidesWithOldestOrder()
        {
            GuideAllQueryModel model = new GuideAllQueryModel()
            {
                SearchString = "neon",
                GuideSorting = (GameCatalouge.Web.ViewModels.Guide.Enums.GuideSorting)1
            };

            AllGuidesFilterServiceModel allGuides = await this.guideService.AllAsync(model);

            Assert.That(allGuides.TotalGuidesCount, Is.EqualTo(this.dbContext.Guides.Count()));
            foreach (var game in allGuides.Guides)
            {
                Assert.IsNotNull(game.Title);
                Assert.IsNotNull(game.AuthorName);
            }
        }

        [Test]
        public async Task AllAsyncShouldReturnAllGuidesWithNewestOrder()
        {
            GuideAllQueryModel model = new GuideAllQueryModel()
            {
                SearchString = "neon",
                GuideSorting = 0
            };

            AllGuidesFilterServiceModel allGuides = await this.guideService.AllAsync(model);

            Assert.That(allGuides.TotalGuidesCount, Is.EqualTo(this.dbContext.Guides.Count()));
            foreach (var game in allGuides.Guides)
            {
                Assert.IsNotNull(game.Title);
                Assert.IsNotNull(game.AuthorName);
            }
        }

        [Test]
        public async Task CreateShouldAddANewGuideToTheDatabase()
        {
            int beforeCount = this.dbContext.Guides.Count();
            string userId = RegularUser.Id.ToString();

            GuideFormModel model = new GuideFormModel()
            {
                Title = Guide.Title,
                Content = Guide.Content
            };

            await this.guideService.Create(model, userId);

            int afterCount = this.dbContext.Guides.Count();

            Assert.That(afterCount, Is.EqualTo(beforeCount + 1));
        }

        [Test]
        public async Task DeleteGuideByIdAsyncShouldDeleteGuide()
        {
            await this.guideService.DeleteGuideByIdAsync(Guide.Id.ToString());

            Assert.IsTrue(Guide.IsDeleted);
        }

        [Test]
        public async Task EditGuideByIdAndFormModelShouldEditGuide()
        {
            GuideFormModel model = new GuideFormModel()
            {
                Title = "This is a new title",
                Content = "New content that is different from the last one"
            };

            string guideId = Guide.Id.ToString();

            await this.guideService.EditGuideByIdAndFormModel(guideId, model);

            Assert.That(Guide.Title, Is.EqualTo(model.Title));
            Assert.That(Guide.Content, Is.EqualTo(model.Content));
        }

        [Test]
        public async Task ExistsByIdAsyncShouldReturnTrueIfTheGuideExists()
        {
            string existingGuideId = Guide.Id.ToString();

            bool result = await this.guideService.ExistsByIdAsync(existingGuideId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task ExistsByIdAsyncShouldReturnFalseIfTheGameDoesntExist()
        {
            string nonExistingGuideId = "-1";

            bool result = await this.guideService.ExistsByIdAsync(nonExistingGuideId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetDetailsByIdAsyncShouldReturnDetailsWithACorrectId()
        {
            string guideId = Guide.Id.ToString();

            GuideDetailsViewModel result = await this.guideService.GetDetailsByIdAsync(guideId);

            Assert.IsNotNull(result.Title);
            Assert.IsNotNull(result.Content);
            Assert.IsNotNull(result.AuthorName);
        }

        [Test]
        public async Task GetGuideDetailsForDeleteByIdAsyncShouldReturnDetailsWithCorrectId()
        {
            string guideId = Guide.Id.ToString();

            GuidePreDeleteViewModel result = await this.guideService.GetGuideDetailsForDeleteByIdAsync(guideId);

            Assert.IsNotNull(result.Title);
            Assert.IsNotNull(result.Content);
        }

        [Test]
        public async Task GetGuideForEditByIdAsyncShouldReturnDetailsWithCorrectId()
        {
            string guideId = Guide.Id.ToString();

            GuideFormModel result = await this.guideService.GetGuideForEditByIdAsync(guideId);

            Assert.IsNotNull(result.Title);
            Assert.IsNotNull(result.Content);
        }

        [Test]
        public async Task IsUserByIdWriterOfGuideByIdShouldReturnTrueWithCorrectData()
        {
            string guideId = Guide.Id.ToString();
            string writerId = RegularUser.Id.ToString();

            bool result = await this.guideService.IsUserByIdWriterOfGuideById(guideId, writerId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsUserByIdWriterOfGuideByIdShouldReturnFalseWithIncorrectData()
        {
            string guideId = Guide.Id.ToString();
            string wrongWriterId = Developer.Id.ToString();

            bool result = await this.guideService.IsUserByIdWriterOfGuideById(guideId, wrongWriterId);

            Assert.IsFalse(result);
        }
    }
}
