using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions option) : base(option) 
        {
            
        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

    }
}
