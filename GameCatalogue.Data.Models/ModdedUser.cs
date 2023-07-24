namespace GameCatalogue.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    public class ModdedUser : IdentityUser<Guid>
    {
        public ModdedUser()
        {
        }
    }
}
