using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels.ProductViewModels
{
    public class ProductUpdateVm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Precision(10, 2)]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public List<int> TagIds { get; set; }
        public IFormFile? MainImage { get; set; }
        public IFormFile? HoverImage { get; set; }
        public List<IFormFile>? Images { get; set; }
        [Range(1, 5)]
        public double? Rating { get; set; }
        public string? MainImagePath { get; set; }
        public string? HoverImagePath { get; set; }
        public List<string>? AdditionalImagePath { get; set; } = [];
        public List<int>? AdditionalImageIds { get; set; } = [];

    }
}
