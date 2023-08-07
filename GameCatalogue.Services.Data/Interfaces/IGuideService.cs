namespace GameCatalogue.Services.Data.Interfaces
{
    using GameCatalogue.Services.Data.Models.Guide;
    using GameCatalouge.Web.ViewModels.Guide;

    public interface IGuideService
    {
        Task<AllGuidesFilterServiceModel> AllAsync(GuideAllQueryModel queryModel);
        Task<string> Create(GuideFormModel formModel, string userId);
    }
}
