namespace GameCatalogue.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static GameCatalogue.Common.EntityValidationConstants.Guide;
    public class Guide
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public Guid AuthorId { get; set; }

        public ModdedUser Author { get; set; }
    }
}
