namespace GameCatalogue.Data.Configurations
{
	using GameCatalogue.Data.Models;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	public class ModdedUserEntityConfiguration : IEntityTypeConfiguration<ModdedUser>
	{
		public void Configure(EntityTypeBuilder<ModdedUser> builder)
		{
			builder
				.Property(u => u.DisplayName)
				.HasDefaultValue("Def");
		}
	}
}
