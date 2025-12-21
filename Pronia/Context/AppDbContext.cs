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

    }
}
