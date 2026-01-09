using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pronia.Configuration
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.Property(x => x.Count).IsRequired();

            builder.ToTable(opt =>
            {
                opt.HasCheckConstraint("CK_Product_Count", "[Count]>0");
            });

            builder.HasIndex(x => new { x.ProductId, x.UserId }).IsUnique();    

            builder.HasOne(x => x.Product).WithMany(x => x.BasketItems).HasForeignKey(x => x.ProductId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User).WithMany(x => x.BasketItems).HasForeignKey(x => x.UserId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);



        }


    }
}
