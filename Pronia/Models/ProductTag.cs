using Pronia.Models.Common;

namespace Pronia.Models
{
    public class ProductTag : BaseEntity
    {
        public Product Product { get; set; } = null!;
        public int ProductId { get; set; }
        public Tag Tag { get; set; } = null!;
        public int TagId { get; set; }


    }
}
