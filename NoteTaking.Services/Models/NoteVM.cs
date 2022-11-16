using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NoteTaking.Services.Models
{
    public class NoteVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [Display(Name = "Note")]
        [DataType(DataType.MultilineText)] 
        public string Content { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public DateTime UpdatedDateTime { get; set; } = DateTime.Now;
        [ValidateNever]
        [JsonIgnore]
        public string UserId { get; set; }
    }
}
