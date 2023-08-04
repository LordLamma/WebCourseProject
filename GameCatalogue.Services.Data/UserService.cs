namespace GameCatalogue.Services.Data
{
	using GameCatalogue.Data;
	using GameCatalogue.Services.Data.Interfaces;
	using GameCatalouge.Web.ViewModels.User;
	using Microsoft.EntityFrameworkCore;

	public class UserService : IUserService
	{
		private readonly GameCatalogueDbContext dbContext;

        public UserService(GameCatalogueDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<AllUserViewModel>> AllAsync()
		{
			List<AllUserViewModel> allUsers = new List<AllUserViewModel>();

			IEnumerable<AllUserViewModel> developers = await this.dbContext
				.Developers
				.Include(d => d.User)
				.Select(d => new AllUserViewModel
				{
					UserName = d.User.UserName,
					Email = d.User.Email,
					BusinessEmail = d.BusinessEmail
				})
				.ToListAsync();

			allUsers.AddRange(developers);

			IEnumerable<AllUserViewModel> users = await this.dbContext
				.Users
				.Where(u => !dbContext.Developers.Any(d => d.UserId == u.Id))
				.Select(u => new AllUserViewModel
				{
					UserName = u.UserName,
					Email = u.Email,
					BusinessEmail = string.Empty
				})
				.ToListAsync();

			allUsers.AddRange(users);

			return allUsers;
		}
	}
}
