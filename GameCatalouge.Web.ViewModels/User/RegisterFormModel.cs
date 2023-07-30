namespace GameCatalouge.Web.ViewModels.User
{
	using System.ComponentModel.DataAnnotations;

	using static GameCatalogue.Common.EntityValidationConstants.User;

	public class RegisterFormModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; } = null!;

		[Required]
		[StringLength(PassWordMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = PassWordMinLength)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; } = null!;

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; } = null!;

		[Required]
		[StringLength(DisplayNameMaxLength, MinimumLength = DisplayNameMinLength)]
		public string DisplayName { get; set; }
	}
}
