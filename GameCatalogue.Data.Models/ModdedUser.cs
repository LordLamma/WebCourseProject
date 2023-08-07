namespace GameCatalogue.Data.Models
{
    using Microsoft.AspNetCore.Identity;
	using System.ComponentModel.DataAnnotations;
    using static GameCatalogue.Common.EntityValidationConstants.User;

	public class ModdedUser : IdentityUser<Guid>
    {
        public ModdedUser()
        {
            this.Id = Guid.NewGuid();
            this.WrittenGuides = new HashSet<Guide>();
        }

        [Required]
        [MaxLength(DisplayNameMaxLength)]
        public string DisplayName { get; set; } = null!;

        public virtual ICollection<Guide> WrittenGuides { get; set; }
    }
}
