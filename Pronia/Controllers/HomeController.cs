using Microsoft.AspNetCore.Mvc;
using Pronia.Context;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var sliders = _context.Sliders.ToList();

            ViewBag.Sliders = sliders;

            var cards = _context.Cards.ToList();


            return View(cards);

        }

    }

}
