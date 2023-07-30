using System.ComponentModel.DataAnnotations;

namespace GameCatalouge.Web.ViewModels.Developer
{
	public class DeveloperDetailsInfoViewModel
	{
		public string DisplayName { get; set; } = null!;

		[Display(Name = "Business e-mail")]
		public string BusinessEmail { get; set; } = null!;
	}
}
