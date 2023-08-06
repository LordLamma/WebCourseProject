namespace GameCatalouge.Web.Areas.Admin.Controllers
{
	using GameCatalogue.Services.Data.Interfaces;
	using GameCatalouge.Web.ViewModels.User;
	using static GameCatalogue.Common.GeneralConstants;

	using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class UserController : BaseAdminController
	{
		private readonly IUserService userService;
		private readonly IMemoryCache memoryCache;

        public UserController(IUserService userService, IMemoryCache memoryCache)
        {
            this.userService = userService;
			this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> All()
		{
			IEnumerable<AllUserViewModel> allUsers = this.memoryCache
				.Get<IEnumerable<AllUserViewModel>>(UsersCacheKey);
			if (allUsers == null)
			{
				allUsers = await this.userService.AllAsync();

				MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromMinutes(UsersCacheExpirationTime));

				this.memoryCache.Set(UsersCacheKey, allUsers, cacheOptions);
            }
			return View(allUsers);
		}
	}
}
