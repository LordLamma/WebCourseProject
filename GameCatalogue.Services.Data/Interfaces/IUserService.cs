namespace GameCatalogue.Services.Data.Interfaces
{
	using GameCatalouge.Web.ViewModels.User;
	public interface IUserService
	{
		Task<IEnumerable<AllUserViewModel>> AllAsync();
	}
}
