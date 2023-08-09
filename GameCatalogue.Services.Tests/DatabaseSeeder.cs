namespace GameCatalogue.Services.Tests
{
    using GameCatalogue.Data;
    using GameCatalogue.Data.Models;

    public static class DatabaseSeeder
    {
        public static ModdedUser RegularUser;
        public static ModdedUser DeveloperUser;
        public static Developer Developer;
        public static Genre GenreOne;
        public static Genre GenreTwo;
        public static Game Game;
        public static Guide Guide;

        public static void SeedDatabase(GameCatalogueDbContext dbContext)
        {
            DeveloperUser = new ModdedUser()
            {
                UserName = "testDev",
                NormalizedUserName = "TESTDEV",
                Email = "testDev@mail.com",
                NormalizedEmail = "TESTDEV@MAIL.COM",
                EmailConfirmed = false,
                PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
                ConcurrencyStamp = "2e652a61-2d24-4d17-8ca8-d5ee610d2803",
                SecurityStamp = "4a36e87b-3f85-46f4-b527-0e9fbda1e226",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                DisplayName = "Def"
            };

            RegularUser = new ModdedUser()
            {
                UserName = "testUser",
                NormalizedUserName = "TESTUSER",
                Email = "testUser@mail.com",
                NormalizedEmail = "TESTUSER@MAIL.COM",
                EmailConfirmed = false,
                PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
                SecurityStamp = "3d1fa0ba-f414-42a4-82df-8dcbfd7271b0",
                ConcurrencyStamp = "f09ed8b3-82b1-45b7-bfcd-6aa441d3f6e8",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                DisplayName = "Def"
            };

            Developer = new Developer()
            {
                BusinessEmail = "testDevBusiness@mail.com",
                User = DeveloperUser
            };

            GenreOne = new Genre()
            {
                Name = "Shooter"
            };

            GenreTwo = new Genre()
            {
                Name = "Roguelike"
            };

            Game = new Game()
            {
                Name = "Neon blast",
                Description = "Fun fast paced shooter game with cool colors",
                ImageURL = "https://img.freepik.com/free-vector/neon-frame-template_23-2149088155.jpg?w=1800&t=st=1691340667~exp=1691341267~hmac=f878f9a376d3ef78758dd57025d6bd0f174b517c6fc25ea0e86147ef5cb1077d",
                Genre = GenreOne,
                Developer = Developer,
            };

            Guide = new Guide()
            {
                Title = "Neon blast quickstart guide",
                Content = "This is a very good guide on how to start neon blast",
                Author = RegularUser
            };

            dbContext.Users.Add(RegularUser);
            dbContext.Users.Add(DeveloperUser);
            dbContext.Developers.Add(Developer);
            dbContext.Genres.Add(GenreOne);
            dbContext.Genres.Add(GenreTwo);
            dbContext.Games.Add(Game);
            dbContext.Guides.Add(Guide);


            dbContext.SaveChanges();
        }
    }
}
