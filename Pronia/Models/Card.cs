using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Card
    {
        public int Id { get; set; }
        [MinLength(2, ErrorMessage ="uzunluq minimum 2 olmalidir.")]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string ImagePath { get; set; } = null!;
    }
}
