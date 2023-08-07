namespace GameCatalogue.Services.Data.Models.Guide
{
    using GameCatalouge.Web.ViewModels.Guide;
    public class AllGuidesFilterServiceModel
    {
        public AllGuidesFilterServiceModel()
        {
            this.Guides = new HashSet<GuideAllViewModel>();
        }

        public int TotalGuidesCount { get; set; }

        public IEnumerable<GuideAllViewModel> Guides { get; set; }
    }
}
