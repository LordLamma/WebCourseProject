namespace GameCatalogue.Services.Data.Interfaces
{
	using GameCatalouge.Web.ViewModels.Developer;

	public interface IDeveloperService
    {
        Task<bool> DeveloperExistsByUserId(string userId);

        Task<bool> DeveloperExistsByEmail(string email);

        Task Create(string  userId, BecomeDeveloperFormModel model);
    }
}
