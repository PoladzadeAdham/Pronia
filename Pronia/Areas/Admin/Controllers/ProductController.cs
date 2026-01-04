using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Context;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.ViewModels.ProductViewModels;
using System.Threading.Tasks;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(AppDbContext context, IWebHostEnvironment environment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var vm = await context.Products.Include(x => x.Category)
                .Select(product => new ProductGetVm()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    CategoryName = product.Category.Name,
                    HoverImagePath = product.HoverImagePath,
                    MainImagePath = product.MainImagePath,
                    Rating = product.Rating,
                    Price = product.Price
                })
                .ToListAsync();

            //List<ProductGetVm> vms = new();

            //foreach (var product in products)
            //{
            //    ProductGetVm vm = new ProductGetVm()
            //    {
            //        Id = product.Id,
            //        Name = product.Name,
            //        Description = product.Description,
            //        CategoryName = product.Category.Name,
            //        HoverImagePath = product.HoverImagePath,
            //        MainImagePath = product.MainImagePath,
            //        Rating = product.Rating,
            //        Price = product.Price,

            //    };

            //    vms.Add(vm);

            //}

            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await context.Products.Include(x => x.Category)
                .Select(product => new ProductGetVm()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    CategoryName = product.Category.Name,
                    HoverImagePath = product.HoverImagePath,
                    MainImagePath = product.MainImagePath,
                    Rating = product.Rating,
                    Price = product.Price,
                    TagNames = product.ProductTags.Select(x => x.Tag.Name).ToList(),
                    AdditionalImagePath = product.ProductImages.Select(x => x.ImagePath).ToList()
                })
                .FirstOrDefaultAsync(x => x.Id == id);


            if (product is null)
                return NotFound();

            return View(product);

        }


        public async Task<IActionResult> Delete(int id)
        {
            var product = await context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);

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


            foreach (var productImage in product.ProductImages)
            {
                string imagePath = Path.Combine(folderPath, productImage.ImagePath);

                ExtensionMethod.DeleteFile(imagePath);

            }


            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await context.Products.Include(x => x.ProductImages).Include(x => x.ProductTags).FirstOrDefaultAsync(x => x.Id == id);

            if (product is null)
                return NotFound();

            await SendItemsWithViewBag();


            ProductUpdateVm vm = new ProductUpdateVm()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Rating = product.Rating,
                MainImagePath = product.MainImagePath,
                HoverImagePath = product.HoverImagePath,
                TagIds = product.ProductTags.Select(t => t.Id).ToList(),
                AdditionalImagePath = product.ProductImages.Select(x => x.ImagePath).ToList(),
                AdditionalImageIds = product.ProductImages.Select(x=>x.Id).ToList(),
            };


            return View(vm);

        }


        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateVm vm)
        {
            await SendItemsWithViewBag();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }




            foreach (var tagId in vm.TagIds)
            {
                var isExistTag = await context.Tags.AnyAsync(t => t.Id == tagId);

                if (!isExistTag)
                {
                    await SendItemsWithViewBag();
                    ModelState.AddModelError("TagIds", "Bele bir tag movcud deil! ");
                    return View();
                }

            }


            if (!vm.MainImage?.CheckType() ?? false)
            {
                ModelState.AddModelError("MainImage", "Yalniz sekil formatinda data daxil etmelisiniz.");
                return View(vm);
            }

            if (!vm.MainImage?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("MainImage", "Max size 2mb olmalidir.");
                return View(vm);
            }

            if (!vm.HoverImage?.CheckType() ?? false)
            {
                ModelState.AddModelError("HoverImage", "Yalniz sekil formatinda data daxil etmelisiniz.");
                return View(vm);
            }

            if (!vm.HoverImage?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("HoverImage", "Max size 2mb olmalidir.");
                return View(vm);
            }


            foreach (var image in vm.Images ?? new())
            {
                if (!image.CheckType())
                {
                    ModelState.AddModelError("HoverImage", "Yalniz sekil formatinda data daxil etmelisiniz.");
                    return View(vm);
                }

                if (!image.CheckSize(2))
                {
                    ModelState.AddModelError("HoverImage", "Max size 2mb olmalidir.");
                    return View(vm);
                }

            }

            var existProduct = await context.Products.Include(x => x.ProductTags).Include(x=>x.ProductImages).FirstOrDefaultAsync(x => x.Id == vm.Id);

            if (existProduct is null)
                return NotFound();

            existProduct.Name = vm.Name;
            existProduct.Description = vm.Description;
            existProduct.Price = vm.Price;
            existProduct.CategoryId = vm.CategoryId;
            existProduct.Rating = vm.Rating;
            existProduct.ProductTags = [];


            foreach (var tagId in vm.TagIds)
            {
                ProductTag productTag = new ProductTag()
                {
                    TagId = tagId,
                    ProductId = existProduct.Id,

                };

                existProduct.ProductTags.Add(productTag);
            }


            string folderPath = Path.Combine(environment.WebRootPath, "assets", "images", "website-images");

            if (vm.MainImage is { })
            {
                string newMainIMmage = await vm.MainImage.SaveFileAsync(folderPath);

                string existMainImagePath = Path.Combine(folderPath, existProduct.MainImagePath);
                ExtensionMethod.DeleteFile(existMainImagePath);

                existProduct.MainImagePath = newMainIMmage;
            }

            if (vm.HoverImage is { })
            {
                string newHoverImage = await vm.HoverImage.SaveFileAsync(folderPath);

                string existHoverImagePath = Path.Combine(folderPath, existProduct.HoverImagePath);
                ExtensionMethod.DeleteFile(existHoverImagePath);

                existProduct.HoverImagePath = newHoverImage;

            }


            List<ProductImage> existImages = existProduct.ProductImages.ToList();

            foreach (var image in existImages)
            {
                var isExistImageId = vm.AdditionalImageIds?.Any(x=>x == image.Id) ?? false;

                if (!isExistImageId)
                {
                    string deletedImagePath = Path.Combine(folderPath, image.ImagePath);
                    ExtensionMethod.DeleteFile(deletedImagePath);
                    existProduct.ProductImages.Remove(image);

                }

            }


            foreach (var image in vm.Images ?? [])
            {
                string uniqueFilePath = await image.SaveFileAsync(folderPath);

                ProductImage productImage = new()
                {
                    ImagePath = uniqueFilePath,
                    ProductId = existProduct.Id
                };

                existProduct.ProductImages.Add(productImage);
            }


            context.Products.Update(existProduct);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");


        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await context.Categories.ToListAsync();

            await SendItemsWithViewBag();

            return View();
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVm vm)
        {

            await SendItemsWithViewBag();
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isExistCategory = await context.Categories.AnyAsync(x => x.Id == vm.CategoryId);


            if (!isExistCategory)
            {
                await SendItemsWithViewBag();

                ModelState.AddModelError("CategoryId", "Bele bir category movcud deil.");
                return View(vm);
            }


            foreach (var tagId in vm.TagIds)
            {
                var isExistTag = await context.Tags.AnyAsync(t => t.Id == tagId);

                if (!isExistTag)
                {
                    await SendItemsWithViewBag();
                    ModelState.AddModelError("TagIds", "Bele bir tag movcud deil! ");
                    return View();
                }

            }


            if (!vm.MainImage.CheckType())
            {
                ModelState.AddModelError("MainImage", "Yalniz sekil formatinda data daxil etmelisiniz.");
                return View(vm);
            }

            if (!vm.MainImage.CheckSize(2))
            {
                ModelState.AddModelError("MainImage", "Max size 2mb olmalidir.");
                return View(vm);
            }

            if (!vm.HoverImage.CheckType())
            {
                ModelState.AddModelError("HoverImage", "Yalniz sekil formatinda data daxil etmelisiniz.");
                return View(vm);
            }

            if (!vm.HoverImage.CheckSize(2))
            {
                ModelState.AddModelError("HoverImage", "Max size 2mb olmalidir.");
                return View(vm);
            }


            foreach (var image in vm.Images)
            {
                if (!image.CheckType())
                {
                    ModelState.AddModelError("HoverImage", "Yalniz sekil formatinda data daxil etmelisiniz.");
                    return View(vm);
                }

                if (!image.CheckSize(2))
                {
                    ModelState.AddModelError("HoverImage", "Max size 2mb olmalidir.");
                    return View(vm);
                }

            }

            string folderPath = Path.Combine(environment.WebRootPath, "assets", "images", "website-images");


            string uniqueMainImageName = await vm.MainImage.SaveFileAsync(folderPath);
            string uniqueHoverImageName = await vm.HoverImage.SaveFileAsync(folderPath);


            Product product = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                CategoryId = vm.CategoryId,
                Price = vm.Price,
                MainImagePath = uniqueMainImageName,
                HoverImagePath = uniqueHoverImageName,
                Rating = vm.Rating,
                ProductTags = [],
                ProductImages = []
            };


            foreach (var image in vm.Images)
            {
                string uniqueFilePath = await image.SaveFileAsync(folderPath);

                ProductImage productImage = new()
                {
                    ImagePath = uniqueFilePath,
                    Product = product
                };

                product.ProductImages.Add(productImage);
            }


            foreach (var tagId in vm.TagIds)
            {
                ProductTag productTag = new()
                {
                    TagId = tagId,
                    Product = product,
                };

                product.ProductTags.Add(productTag);
            }


            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        private async Task SendItemsWithViewBag()
        {
            var categories = await context.Categories.ToListAsync();

            ViewBag.Categories = categories;

            var tags = await context.Tags.ToListAsync();

            ViewBag.Tags = tags;

        }
    }
}
