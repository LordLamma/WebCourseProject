namespace GameCatalogue.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using GameCatalogue.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using System.Reflection;

    public class GameCatalogueDbContext : IdentityDbContext<ModdedUser, IdentityRole<Guid>, Guid>
    {
        public GameCatalogueDbContext(DbContextOptions<GameCatalogueDbContext> options)
            : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; } = null!;

        public DbSet<Game> Games { get; set; } = null!;

        public DbSet<Developer> Developers { get; set; } = null!;

        public DbSet<Guide> Guides { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(GameCatalogueDbContext)) ?? 
                                Assembly.GetExecutingAssembly();
            builder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(builder);
        }
    }
}