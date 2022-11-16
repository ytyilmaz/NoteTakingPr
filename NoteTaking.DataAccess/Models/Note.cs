using System.ComponentModel.DataAnnotations;

namespace NoteTaking.DataAccess
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public DateTime UpdatedDateTime { get; set; } = DateTime.Now;
        public string UserId { get; set; }
    }
}
