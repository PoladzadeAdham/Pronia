using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Context;
using Pronia.Models;
using System.Threading.Tasks;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController(AppDbContext _context) : Controller
    {

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.ToListAsync();

            return View(sliders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (slider.OfferPercentage < 0 || slider.OfferPercentage > 100)
            {
                ModelState.AddModelError("OfferPercentage", "deyer 0 100 araliginda olmalidir.");
                return View();
            }

            var exist = await _context.Sliders.AnyAsync(x => x.Title == slider.Title);

            if (exist)
            {
                ModelState.AddModelError("Title", "Bu title da slider movcuddur.");
                return View();
            }

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);

            if (slider is null)
            {
                return NotFound();
            }

            return View(slider);

        }

        [HttpPost]
        public async Task<IActionResult> Update(Slider slider)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var existSlider = await _context.Sliders.FindAsync(slider.Id);

            if (existSlider is null)
                return BadRequest();


            existSlider.Title = slider.Title;
            existSlider.Description = slider.Description;
            existSlider.OfferPercentage = slider.OfferPercentage;
            existSlider.ImagePath = slider.ImagePath;

            _context.Sliders.Update(existSlider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
