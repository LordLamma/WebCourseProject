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
                .Property(x => x.IsDeleted)
                .HasDefaultValue(false);

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
        }
    }
}
