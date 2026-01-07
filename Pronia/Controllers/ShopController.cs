using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Abstractions;
using Pronia.Context;
using Pronia.ViewModels.ProductViewModels;
using System.Threading.Tasks;

namespace Pronia.Controllers
{
    public class ShopController(AppDbContext context, IEmailService emailService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await context.Products.ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Test()
        {
            await emailService.SendEmailAsync("edhempoladzade2@gmail.com", "Mpa-101", "<h1 style:'color:red'> Email service is done </h1>");

            return Ok("Ok");
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var product = await context.Products
                .Select(x => new ProductGetVm()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Rating = x.Rating,
                    Description = x.Description,
                    AdditionalImagePath = x.ProductImages.Select(x => x.ImagePath).ToList(),
                    CategoryName = x.Category.Name,
                    HoverImagePath = x.HoverImagePath,
                    MainImagePath = x.MainImagePath,
                    Price = x.Price,
                    TagNames = x.ProductTags.Select(x => x.Tag.Name).ToList()
                })
                .FirstOrDefaultAsync(x => x.Id == id);


            if (product == null)
            {
                return NotFound();
            }


            return View(product);

        }


    }
}
