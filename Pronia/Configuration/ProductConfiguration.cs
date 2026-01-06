using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pronia.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(256);

            builder.Property(x => x.Price).HasPrecision(10, 2).IsRequired();

            builder.HasCheckConstraint("CK_Product_Price", "[Price]>0");

            //builder.ToTable(x => x.HasCheckConstraint("CK_Product_Name", "LEN([Name]) > 3"));

            builder.Property(x => x.CategoryId).IsRequired();


            builder.HasOne(x => x.Category)
                   .WithMany(x => x.Products)
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(x=>x.ProductImages)
                   .WithOne(x=>x.Product).HasForeignKey(x=>x.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(x=>x.ProductTags)
                   .WithOne(x=>x.Product)
                   .HasForeignKey(x=>x.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

        }

    }


}
