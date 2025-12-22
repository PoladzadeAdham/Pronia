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


            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }

    }
}
