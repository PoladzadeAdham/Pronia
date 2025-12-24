using Microsoft.EntityFrameworkCore;
using Pronia.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [MaxLength(512)]
        public string ImagePath { get; set; }
        [Required]
        [Precision(10,2)]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public Category? Category { get; set; }
        public int CategoryId { get; set; }


    }

}

