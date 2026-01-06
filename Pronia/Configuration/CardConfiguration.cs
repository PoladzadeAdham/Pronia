using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pronia.Configuration
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {

            builder.Property(x => x.Title).IsRequired().HasMaxLength(256);

            builder.Property(x=>x.Description).IsRequired();

            builder.Property(x=>x.ImagePath).IsRequired();

        }
    }
}
