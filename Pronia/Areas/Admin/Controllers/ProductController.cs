using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Context;
using Pronia.ViewModels.ProductViewModels;
using System.Threading.Tasks;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(AppDbContext context, IWebHostEnvironment environment) : Controller
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

            string folderPath = Path.Combine(environment.WebRootPath, "assets", "images", "website-images");

            string mainImagePath = Path.Combine(folderPath, product.MainImagePath);
            string hoverImagePath = Path.Combine(folderPath, product.HoverImagePath);


            if (System.IO.File.Exists(mainImagePath))
                System.IO.File.Delete(mainImagePath);

            if (System.IO.File.Exists(hoverImagePath))
                System.IO.File.Delete(hoverImagePath);



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

            if (existProduct is null)
                return NotFound();

            existProduct.Name = product.Name;
            existProduct.Description = product.Description;
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
        public async Task<IActionResult> Create(ProductCreateVm vm)
        {

            await SendCategoriesWithViewBag();
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isExistCategory = await context.Categories.AnyAsync(x => x.Id == vm.CategoryId);

            if (!isExistCategory)
            {
                await SendCategoriesWithViewBag();

                ModelState.AddModelError("CategoryId", "Bele bir category movcud deil.");
                return View(vm);
            }

            if (vm.MainImage.ContentType.Contains("Image"))
            {
                ModelState.AddModelError("MainImage", "Yalniz sekil formatinda data daxil etmelisiniz.");
                return View(vm);
            }

            if (vm.MainImage.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("MainImage", "Max size 2mb olmalidir.");
                return View(vm);
            }

            if (vm.HoverImage.ContentType.Contains("Image"))
            {
                ModelState.AddModelError("HoverImage", "Yalniz sekil formatinda data daxil etmelisiniz.");
                return View(vm);
            }

            if (vm.HoverImage.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("HoverImage", "Max size 2mb olmalidir.");
                return View(vm);
            }


            string uniqueMainImageName = Guid.NewGuid().ToString() + vm.MainImage.FileName;
            string mainImagePath = Path.Combine(environment.WebRootPath, "assets", "images", "website-images", uniqueMainImageName);


            using FileStream mainStream = new FileStream(mainImagePath, FileMode.Create);

            await vm.MainImage.CopyToAsync(mainStream);


            string uniqueHoverImageName = Guid.NewGuid().ToString() + vm.HoverImage.FileName;
            string hoverImagePath = Path.Combine(environment.WebRootPath, "assets", "images", "website-images", uniqueHoverImageName);


            using FileStream hoverStream = new FileStream(hoverImagePath, FileMode.Create);

            await vm.HoverImage.CopyToAsync(hoverStream);

            Product product = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                CategoryId = vm.CategoryId,
                Price = vm.Price,
                MainImagePath = uniqueMainImageName,
                HoverImagePath = uniqueHoverImageName,
                Rating = vm.Rating,
            };


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
