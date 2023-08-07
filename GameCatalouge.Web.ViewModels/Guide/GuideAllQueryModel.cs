namespace GameCatalouge.Web.ViewModels.Guide
{
    using GameCatalouge.Web.ViewModels.Guide.Enums;
    using static GameCatalogue.Common.GeneralConstants;

    using System.ComponentModel.DataAnnotations;

    public class GuideAllQueryModel
    {
        public GuideAllQueryModel()
        {
            this.CurrentPage = StartingPage;
            this.GuidesPerPage = AllowedGuidesPerPage;
        }

        [Display(Name = "Search by a keyword")]
        public string? SearchString { get; set; }

        [Display(Name = "Sort by")]
        public GuideSorting GuideSorting { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "How many guides on page")]
        public int GuidesPerPage { get; set; }

        public int TotalGuides { get; set; }

        public IEnumerable<GuideAllViewModel> Guides { get; set; }
    }
}
