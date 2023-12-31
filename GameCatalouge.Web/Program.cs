namespace GameCatalouge.Web
{
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using GameCatalogue.Data;
	using GameCatalogue.Data.Models;
	using GameCatalogue.Services.Data;
	using GameCatalogue.Web.Infrastructure.Extensions;
	using GameCatalouge.Web.Controllers;
	using Microsoft.AspNetCore.Mvc;
	using static GameCatalogue.Common.GeneralConstants;

	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<GameCatalogueDbContext>(options =>
				options.UseSqlServer(connectionString));

			builder.Services.AddDefaultIdentity<ModdedUser>(options =>
			{
				options.SignIn.RequireConfirmedAccount =
					builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
				options.Password.RequireNonAlphanumeric =
					builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
				options.Password.RequireLowercase =
					builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
				options.Password.RequireUppercase =
					builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
				options.Password.RequireDigit =
					builder.Configuration.GetValue<bool>("Identity:Password:RequireDigit");
				options.Password.RequiredLength =
					builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
			})
				.AddRoles<IdentityRole<Guid>>()
				.AddEntityFrameworkStores<GameCatalogueDbContext>();

			builder.Services.AddApplicationServices(typeof(GameService));

            builder.Services.AddMemoryCache();

            builder.Services.ConfigureApplicationCookie(cnf =>
			{
				cnf.LoginPath = "/User/Login";
			});

			builder.Services
				.AddControllersWithViews()
				.AddMvcOptions(options =>
				{
					options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
				});


			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error/500");
				app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(config =>
			{
				config.MapControllerRoute(
				  name: "areas",
				  pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}");
				config.MapDefaultControllerRoute();
				config.MapRazorPages();
			});

			app.Run();
		}
	}
}