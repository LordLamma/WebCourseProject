namespace GameCatalogue.Services.Tests
{
    using GameCatalogue.Data;
    using GameCatalogue.Services.Data.Interfaces;
    using GameCatalogue.Services.Data;
    using static GameCatalogue.Services.Tests.DatabaseSeeder;
    using GameCatalouge.Web.ViewModels.Game;
    using GameCatalogue.Services.Data.Models.Game;

    using Microsoft.EntityFrameworkCore;

    internal class GameServiceTests
    {
        private DbContextOptions<GameCatalogueDbContext> dbOptions;
        private GameCatalogueDbContext dbContext;

        private IGameService gameService;

        [SetUp]
        public void SetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<GameCatalogueDbContext>()
                .UseInMemoryDatabase("GameCatalogueInMemory" + Guid.NewGuid().ToString())
                .Options;
            this.dbContext = new GameCatalogueDbContext(this.dbOptions);

            dbContext.Database.EnsureCreated();

            SeedDatabase(this.dbContext);

            this.gameService = new GameService(this.dbContext);
        }

        [Test]
        public async Task ExistsByIdAsyncShouldReturnTrueIfTheGameExists()
        {
            string existingGameID = Game.Id.ToString();

            bool result = await this.gameService.ExistsByIdAsync(existingGameID);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task ExistsByIdAsyncShouldReturnFalseIfTheGameDoesntExist()
        {
            string nonExistingGameID = "2";

            bool result = await this.gameService.ExistsByIdAsync(nonExistingGameID);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetDetailsByIdAsyncShouldReturnDetailsWithACorrectId()
        {
            string gameId = Game.Id.ToString();

            GameDetailsViewModel result = await this.gameService.GetDetailsByIdAsync(gameId);

            Assert.IsNotNull(result.Name);
            Assert.IsNotNull(result.ImageURL);
            Assert.IsNotNull(result.Genre);
            Assert.IsNotNull(result.Description);
            Assert.IsNotNull(result.Developer);
        }

        [Test]
        public async Task GetGameDetailsForDeleteByIdAsyncShouldReturnDetailsWithCorrectId()
        {
            string gameId = Game.Id.ToString();

            GamePreDeleteDetailsViewModel result = await this.gameService.GetGameDetailsForDeleteByIdAsync(gameId);

            Assert.IsNotNull(result.Name);
            Assert.IsNotNull(result.Description);
            Assert.IsNotNull(result.ImageURL);
        }

        [Test]
        public async Task GetGameForEditByIdAsyncShouldReturnDetailsWithCorrectId()
        {
            string gameId = Game.Id.ToString();

            GameFormModel result = await this.gameService.GetGameForEditByIdAsync(gameId);

            Assert.IsNotNull(result.Name);
            Assert.IsNotNull(result.Description);
            Assert.IsNotNull(result.ImageURL);
            Assert.IsNotNull(result.GenreId);
            Assert.IsNotNull(result.Genres);
        }

        [Test]
        public async Task IsDeveloperByIdProducerOfGameByIdAsyncShouldReturnTrueWithCorrectData()
        {
            string gameId = Game.Id.ToString();
            string devId = Developer.Id.ToString();

            bool result = await this.gameService.IsDeveloperByIdProducerOfGameByIdAsync(gameId,devId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsDeveloperByIdProducerOfGameByIdAsyncShouldReturnFalseWithIncorrectData()
        {
            string gameId = Game.Id.ToString();
            string userId = RegularUser.Id.ToString();

            bool result = await this.gameService.IsDeveloperByIdProducerOfGameByIdAsync(gameId, userId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task EditGameByIdAndFormModelShouldEditGame()
        {
            GameFormModel model = new GameFormModel()
            {
                Name = "name",
                Description = "this is a description",
                ImageURL = "https://img.freepik.com/free-vector/neon-frame-template_23-2149088155.jpg?w=1800&t=st=1691340667~exp=1691341267~hmac=f878f9a376d3ef78758dd57025d6bd0f174b517c6fc25ea0e86147ef5cb1077d",
                GenreId = 1
            };

            string gameId = Game.Id.ToString();

            await this.gameService.EditGameByIdAndFormModel(gameId, model);

            Assert.That(Game.Name, Is.EqualTo(model.Name));
            Assert.That(Game.Description, Is.EqualTo(model.Description));
            Assert.That(Game.ImageURL, Is.EqualTo(model.ImageURL));
            Assert.That(Game.GenreId, Is.EqualTo(model.GenreId));
        }

        [Test]
        public async Task DeleteGameByIdAsyncShouldDeleteGame()
        {
            await this.gameService.DeleteGameByIdAsync(Game.Id.ToString());

            Assert.IsTrue(Game.IsDeleted);
        }

        [Test]
        public async Task CreateShouldAddANewGameToTheDatabase()
        {
            int beforeCount = this.dbContext.Games.Count();
            string devId = Developer.Id.ToString();

            GameFormModel model = new GameFormModel()
            {
                Name = Game.Name,
                Description = Game.Description,
                ImageURL = Game.ImageURL,
                GenreId = Game.GenreId
            };

            await this.gameService.Create(model, devId);

            int afterCount = this.dbContext.Games.Count();

            Assert.That(afterCount,Is.EqualTo(beforeCount + 1));
        }

        [Test]
        public async Task AllByDeveloperIdAsyncShouldReturnAListWithGamesByThatDeveloperOnly()
        {
            string devId = Developer.Id.ToString();

            IEnumerable<GameAllViewModel> games = await this.gameService.AllByDeveloperIdAsync(devId);

            bool result = true;

            foreach (var game in Developer.MadeGames)
            {
                if (!(games.Any(g => g.Name == game.Name)))
                {
                    result = false;
                }
            }

            Assert.IsTrue(result);
        }

        [Test]
        public async Task AllAsyncShouldReturnAllGamesWithNewestOrder()
        {
            AllGamesQueryModel model = new AllGamesQueryModel()
            {
                Genre = GenreOne.Name,
                SearchString = "neon",
                GameSorting = (GameCatalouge.Web.Views.Game.Enums.GameSorting)1
            };

            AllGamesFilterServiceModel allGames = await this.gameService.AllAsync(model);

            Assert.That(allGames.TotalGamesCount, Is.EqualTo(this.dbContext.Games.Count()));
            foreach (var game in allGames.Games)
            {
                Assert.IsNotNull(game.Name);
                Assert.IsNotNull(game.ImageURL);
                Assert.IsNotNull(game.Genre);
            }
        }

        [Test]
        public async Task AllAsyncShouldReturnAllGamesOldestOrder()
        {
            AllGamesQueryModel model = new AllGamesQueryModel()
            {
                Genre = GenreOne.Name,
                SearchString = "neon",
                GameSorting = (GameCatalouge.Web.Views.Game.Enums.GameSorting)2
            };

            AllGamesFilterServiceModel allGames = await this.gameService.AllAsync(model);

            Assert.That(allGames.TotalGamesCount, Is.EqualTo(this.dbContext.Games.Count()));
            foreach (var game in allGames.Games)
            {
                Assert.IsNotNull(game.Name);
                Assert.IsNotNull(game.ImageURL);
                Assert.IsNotNull(game.Genre);
            }
        }

        [Test]
        public async Task AllAsyncShouldReturnAllGamesAlphabeticalOrder()
        {
            AllGamesQueryModel model = new AllGamesQueryModel()
            {
                Genre = GenreOne.Name,
                SearchString = "neon",
                GameSorting = (GameCatalouge.Web.Views.Game.Enums.GameSorting)0
            };

            AllGamesFilterServiceModel allGames = await this.gameService.AllAsync(model);

            Assert.That(allGames.TotalGamesCount, Is.EqualTo(this.dbContext.Games.Count()));
            foreach (var game in allGames.Games)
            {
                Assert.IsNotNull(game.Name);
                Assert.IsNotNull(game.ImageURL);
                Assert.IsNotNull(game.Genre);
            }
        }

        [Test]
        public async Task AllAsyncShouldReturnAllGamesDefaultOrder()
        {
            AllGamesQueryModel model = new AllGamesQueryModel()
            {
                Genre = GenreOne.Name,
                SearchString = "neon"
            };

            AllGamesFilterServiceModel allGames = await this.gameService.AllAsync(model);

            Assert.That(allGames.TotalGamesCount, Is.EqualTo(this.dbContext.Games.Count()));
            foreach (var game in allGames.Games)
            {
                Assert.IsNotNull(game.Name);
                Assert.IsNotNull(game.ImageURL);
                Assert.IsNotNull(game.Genre);
            }
        }

    }
}
