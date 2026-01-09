using Pronia.Models.Common;

namespace Pronia.Models
{
    public class BasketItem : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Count { get; set; }
        public AppUser User { get; set; } = null!;
        public string UserId { get; set; } = null!;


    }
}
