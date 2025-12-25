using Pronia.Models.Common;

namespace Pronia.Models
{

    public class ProductImage : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string ImagePath { get; set; }

    }
}
