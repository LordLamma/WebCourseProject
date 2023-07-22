namespace GameCatalogue.Data.Configurations
{
    using GameCatalogue.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using static GameCatalogue.Common.EntityValidationConstants;
    using Game = Models.Game;

    public class GameEntityConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("GETDATE()");

            builder
                .HasOne(ga => ga.Genre)
                .WithMany(ge => ge.Games)
                .HasForeignKey(ga => ga.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ga => ga.Developer)
                .WithMany(d => d.MadeGames)
                .HasForeignKey(ga => ga.DeveloperId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(this.GenerateGames());
        }

        private Game[] GenerateGames()
        {
            ICollection<Game> games = new HashSet<Game>();

            Game game;

            game = new Game()
            {
                Id = 1,
                Name = "Neon blast",
                Description = "Neon Blast: Enter a cybernetic world of neon-lit chaos. Unleash your skills, wield futuristic weapons, and fight against a corrupt regime. Fast-paced action, stunning visuals, and intense multiplayer battles await!",
                ImageURL = "https://media.istockphoto.com/id/993696960/photo/emoticon-smile-led.jpg?s=2048x2048&w=is&k=20&c=WzuM-npePurbItkdrOsVxy6PlWUaUu37MdGJDzUkVxQ=",
                GenreId = 1,
                DeveloperId = Guid.Parse("9C9E3599-5D8C-4E84-9FE9-97528E3B3025")
            };
            games.Add(game);

            game = new Game()
            {
                Id = 2,
                Name = "Dungeoneer",
                Description = "\"Dungeoneer\": Deadly dungeons, treasures await. Procedural, perilous. Battle, survive. Unravel secrets, embrace challenge. Permadeath, endless exploration. Conquer or fall. Good luck!",
                ImageURL = "https://media.istockphoto.com/id/1386931686/vector/dungeon-long-medieval-castle-corridor-with-torches-interior-of-ancient-palace-with-stone.jpg?s=2048x2048&w=is&k=20&c=kJEOjgDceG-1HjRWvFTEy7BGYoN0OX8sdygWppl_PYU=",
                GenreId = 2,
                DeveloperId = Guid.Parse("9C9E3599-5D8C-4E84-9FE9-97528E3B3025")
            };
            games.Add(game);


            return games.ToArray();
        }
    }
}
