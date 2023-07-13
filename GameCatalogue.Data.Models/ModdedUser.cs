namespace GameCatalogue.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    public class ModdedUser : IdentityUser<Guid>
    {
        public ModdedUser()
        {
            this.WishlistedGames = new HashSet<UserGame>();
        }

        public virtual ICollection<UserGame> WishlistedGames { get; set; }
    }
}
