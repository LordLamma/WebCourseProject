namespace GameCatalogue.Data.Configurations
{
    using GameCatalogue.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class GuideEntityConfiguration : IEntityTypeConfiguration<Guide>
    {
        public void Configure(EntityTypeBuilder<Guide> builder)
        {
            builder
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("GETDATE()");

            builder
                .Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder
                .HasOne(gu => gu.Author)
                .WithMany(a => a.WrittenGuides)
                .HasForeignKey(gu => gu.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
