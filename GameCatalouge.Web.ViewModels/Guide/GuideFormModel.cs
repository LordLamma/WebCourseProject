namespace GameCatalouge.Web.ViewModels.Guide
{
    using System.ComponentModel.DataAnnotations;
    using static GameCatalogue.Common.EntityValidationConstants.Guide;

    public class GuideFormModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; set; } = null!;
    }
}
