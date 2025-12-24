using Pronia.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }

}

