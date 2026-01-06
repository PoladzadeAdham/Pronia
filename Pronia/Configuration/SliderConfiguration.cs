using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pronia.Configuration
{
    public class SliderConfiguration : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.ImagePath).IsRequired();
            builder.Property(x => x.OfferPercentage).HasPrecision(10, 2);


        }
    }
}
