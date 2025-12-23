using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Context;
using Pronia.Models;
using System.Threading.Tasks;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CardController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var cards = await _context.Cards.ToListAsync();

            return View(cards);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var card = await _context.Cards.FindAsync(id);

            if(card is null)
            {
                return NotFound();
            }

            return View(card);

        }

        [HttpPost]
        public async Task<IActionResult> Update(Card card)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existCard = await _context.Cards.FindAsync(card.Id);

            if(existCard is null)
                return NotFound();


            existCard.Title = card.Title;
            existCard.Description = card.Description;
            existCard.ImagePath = card.ImagePath;

            _context.Cards.Update(existCard);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Card card)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _context.AddAsync(card);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var card = await _context.Cards.FindAsync(id);

            if(card is null)
            {
                return NotFound();
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

    }
}
