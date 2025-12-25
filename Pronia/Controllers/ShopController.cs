using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Context;
using System.Threading.Tasks;

namespace Pronia.Controllers
{
    public class ShopController(AppDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await context.Products.ToListAsync();

            return View(products);
        }



    }
}
