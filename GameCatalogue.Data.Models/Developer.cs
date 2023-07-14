namespace GameCatalogue.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static GameCatalogue.Common.EntityValidationConstants.Developer;
    public  class Developer
    {
        public Developer()
        {
            this.Id = Guid.NewGuid();
            this.MadeGames = new HashSet<Game>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(EmailMaxLenght)]
        public string BusinessEmail { get; set; } = null!;

        public Guid UserId { get; set; }

        public virtual ModdedUser User { get; set; } = null!;

        public virtual ICollection<Game> MadeGames { get; set; }
    }
}
