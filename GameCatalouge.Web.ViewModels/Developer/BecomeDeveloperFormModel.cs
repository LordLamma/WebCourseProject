namespace GameCatalouge.Web.ViewModels.Developer
{
	using System.ComponentModel.DataAnnotations;
    using static GameCatalogue.Common.EntityValidationConstants.Developer;
	public class BecomeDeveloperFormModel
    {
        [Required]
        [StringLength(EmailMaxLenght, MinimumLength = EmailMinLenght)]
        [RegularExpression(EmailRegEx, ErrorMessage = "Invalid email adress")]
        [Display(Name = "Business e-mail")]
        public string BusinessEmail { get; set; } = null!;
    }
}
