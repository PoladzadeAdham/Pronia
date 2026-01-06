using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Pronia.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(256); 

            builder.HasMany(x => x.ProductTags).WithOne(x => x.Tag).HasForeignKey(x => x.TagId);

        }



    }
}
