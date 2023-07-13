namespace GameCatalogue.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Comment
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
