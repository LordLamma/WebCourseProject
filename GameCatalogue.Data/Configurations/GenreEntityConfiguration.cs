namespace GameCatalogue.Data.Configurations
{
    using GameCatalogue.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    internal class GenreEntityConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasData(this.GenerateGenres());
        }

        private Genre[] GenerateGenres()
        {
            ICollection<Genre> genres = new HashSet<Genre>();

            Genre genre;

            genre = new Genre()
            {
                Id = 1,
                Name = "Shooter"
            };
            genres.Add(genre);

            genre = new Genre()
            {
                Id = 2,
                Name = "Roguelike"
            };
            genres.Add(genre);

            genre = new Genre()
            {
                Id = 3,
                Name = "Platformer"
            };
            genres.Add(genre);

            genre = new Genre()
            {
                Id = 4,
                Name = "Battle royale"
            };
            genres.Add(genre);

            genre = new Genre()
            {
                Id = 5,
                Name = "Puzzle"
            };
            genres.Add(genre);

            genre = new Genre()
            {
                Id = 6,
                Name = "Adventure"
            };
            genres.Add(genre);

            return genres.ToArray();
        }
    }
}
