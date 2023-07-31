namespace GameCatalogue.Web.Infrastructure.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using GameCatalogue.Services.Data;
    using GameCatalogue.Services.Data.Interfaces;
    using System.Reflection;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Identity;
	using GameCatalogue.Data.Models;
    using static GameCatalogue.Common.GeneralConstants;

	public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// This method registers all services with their interfaces and implementations of given assembly.
        /// The assembly is taken from the type of the provided service implementation.
        /// </summary>
        /// <param name="serviceType">A type of any service sholud be provided.</param>
        /// <exception cref="InvalidOperationException"></exception>

        public static void AddApplicationServices(this IServiceCollection services, Type serviceType)
        {
            Assembly? serviceAssebmly = Assembly.GetAssembly(serviceType);
            if (serviceAssebmly == null)
            {
                throw new InvalidOperationException("Invalid service type");
            }
            Type[] serviceTypes = serviceAssebmly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
                .ToArray();
            foreach (Type st in serviceTypes)
            {
                Type? interfaceType = st.GetInterface($"I{st.Name}");
                if (interfaceType == null)
                {
                    throw new InvalidOperationException($"Invalid interface input for the service with name: {st.Name}");
                }

                services.AddScoped(interfaceType, st);
            }
            services.AddScoped<IGameService, GameService>();
        }
		public static IApplicationBuilder SeedAdmin(this IApplicationBuilder app, string email)
		{
            using IServiceScope scopedServices = app.ApplicationServices.CreateScope();

            IServiceProvider serviceProvider = scopedServices.ServiceProvider;

            UserManager<ModdedUser> userManager = serviceProvider
                .GetRequiredService<UserManager<ModdedUser>>();

            RoleManager<IdentityRole<Guid>> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(AdminRoleName))
                {
                    return;
                }

                IdentityRole<Guid> role = new IdentityRole<Guid>(AdminRoleName);

                await roleManager.CreateAsync(role);

                ModdedUser adminUser = await userManager.FindByEmailAsync(email);

                await userManager.AddToRoleAsync(adminUser, AdminRoleName);
            })
            .GetAwaiter()
            .GetResult();

            return app;
		}
	}
}
