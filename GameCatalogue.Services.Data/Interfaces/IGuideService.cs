namespace GameCatalogue.Services.Data.Interfaces
{
    using GameCatalogue.Services.Data.Models.Guide;
    using GameCatalouge.Web.ViewModels.Guide;

    public interface IGuideService
    {
        Task<AllGuidesFilterServiceModel> AllAsync(GuideAllQueryModel queryModel);
        Task<string> Create(GuideFormModel formModel, string userId);
        Task<bool> ExistsByIdAsync(string id);
        Task<GuideFormModel> GetGuideForEditByIdAsync(string gameId);
        Task<GuideDetailsViewModel?> GetDetailsByIdAsync(string id);
        Task<bool> IsUserByIdWriterOfGuideById(string guideId, string userId);
        Task EditGuideByIdAndFormModel(string guideId, GuideFormModel formModel);
        Task<GuidePreDeleteViewModel> GetGuideDetailsForDeleteByIdAsync(string gameId);
        Task DeleteGuideByIdAsync(string guideId);
    }
}
