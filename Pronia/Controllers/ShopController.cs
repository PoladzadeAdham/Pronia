using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Abstractions;
using Pronia.Context;
using Pronia.ViewModels.ProductViewModels;
using System.Security.Claims;
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

        [Authorize]
        public async Task<IActionResult> AddToBasket(int productId)
        {
            var isExistProduct = await context.Products.AnyAsync(x => x.Id == productId);

            if (isExistProduct == false)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            var isExistUser = await context.Users.AnyAsync(x => x.Id == userId);

            if (isExistUser == false)
                return BadRequest();


            var existBasketItem = await context.BasketItems.FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

            if (existBasketItem != null)
            {
                existBasketItem.Count++;

                context.Update(existBasketItem);

                await context.SaveChangesAsync();
            }
            else
            {
                BasketItem basketItem = new()
                {
                    ProductId = productId,
                    UserId = userId,
                    Count = 1
                };

                await context.BasketItems.AddAsync(basketItem);
            }

            await context.SaveChangesAsync();

            string? returnUrl = Request.Headers["Referer"];

            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index");

        }


        [Authorize]
        public async Task<IActionResult> RemoveFromBasket(int productId)
        {
            var isExistProduct = await context.Products.AnyAsync(x => x.Id == productId);

            if (isExistProduct == false)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            var isExistUser = await context.Users.AnyAsync(x => x.Id == userId);

            if (isExistUser == false)
                return BadRequest();

            var basketItem = await context.BasketItems.FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

            if (basketItem == null)
                return NotFound();

            context.BasketItems.Remove(basketItem);

            await context.SaveChangesAsync();

            string? returnUrl = Request.Headers["Referer"];

            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index");

        }

    }
}
