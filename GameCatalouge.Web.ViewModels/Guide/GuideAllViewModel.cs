namespace GameCatalouge.Web.ViewModels.Guide
{
    using System.ComponentModel.DataAnnotations;

    public class GuideAllViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        [Display(Name = "Author")]
        public string AuthorName { get; set; } = null!;
    }
}
