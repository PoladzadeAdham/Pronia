using Microsoft.EntityFrameworkCore;
using Pronia.Context;

namespace Pronia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            var app = builder.Build();

            app.MapDefaultControllerRoute();

            app.UseStaticFiles();

            app.Run();
        }
    }
}
