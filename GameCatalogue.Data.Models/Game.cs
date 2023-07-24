namespace GameCatalogue.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.Game;
    
    public class Game
    {
        public Game()
        {
            //this.Comments = new HashSet<Comment>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLenght)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLenght)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(ImageURLMaxLength)]

        public string ImageURL { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; } = null!;

        public Guid DeveloperId { get; set; }

        public virtual Developer Developer { get; set; } = null!;
    }
}
