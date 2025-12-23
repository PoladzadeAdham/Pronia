using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [MinLength(2)]
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Required]
        public string ImagePath { get; set; } = null!;
        public decimal OfferPercentage { get; set; }

    }
}
