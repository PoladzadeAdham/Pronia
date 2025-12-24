using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Context;
using System.Threading.Tasks;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(AppDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await context.Products.Include(x => x.Category).ToListAsync();

            return View(products);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var product = await context.Products.FindAsync(id);

            if (product is null)
                return NotFound();

            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await context.Products.FindAsync(id);

            if (product is null)
                return NotFound();

            await SendCategoriesWithViewBag();

            return View(product);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            if (!ModelState.IsValid)
            {
                await SendCategoriesWithViewBag();
                return View(product);
            }

            var existProduct = await context.Products.FindAsync(product.Id);

            if(existProduct is null)
                return NotFound();

            existProduct.Name = product.Name;
            existProduct.Description = product.Description;
            existProduct.ImagePath = product.ImagePath;
            existProduct.Price = product.Price;
            existProduct.CategoryId = product.CategoryId;

            context.Products.Update(existProduct);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");


        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await context.Categories.ToListAsync();

            ViewBag.Categories = categories;

            return View();
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {

            if (!ModelState.IsValid)
            {
                await SendCategoriesWithViewBag();
                return View();
            }

            var isExistCategory = await context.Categories.AnyAsync(x => x.Id == product.CategoryId);

            if (!isExistCategory)
            {
                await SendCategoriesWithViewBag();

                ModelState.AddModelError("CategoryId", "Bele bir category movcud deil.");
                return View(product);
            }

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        private async Task SendCategoriesWithViewBag()
        {
            var categories = await context.Categories.ToListAsync();

            ViewBag.Categories = categories;
        }
    }
}
