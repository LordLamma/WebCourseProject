namespace GameCatalogue.Data.Models
{
    public class UserGame
    {
        public Guid UserId { get; set; }
        public ModdedUser User { get; set; } = null!;


        public int GameId { get; set; }
        public Game Game { get; set; } = null!;
    }
}
