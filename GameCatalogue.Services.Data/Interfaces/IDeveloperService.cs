namespace GameCatalogue.Services.Data.Interfaces
{
    public interface IDeveloperService
    {
        Task<bool> DeveloperExistsByUserId(string userId);
    }
}
