using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Abstractions;
using Pronia.Context;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pronia.Controllers
{
    public class BasketController(IBasketService _basketService, AppDbContext _appDbContext) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var basketItems = await _basketService.GetBasketItemsAsync();

            return View(basketItems);
        }


        public async Task<IActionResult> DecreaseBasketItemCount(int productId)
        {

            var isExistProduct = await _appDbContext.Products.AnyAsync(x => x.Id == productId);

            if (isExistProduct == false)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            var isExistUser = await _appDbContext.Users.AnyAsync(x => x.Id == userId);

            if (isExistUser == false)
                return BadRequest();

            var basketItem = await _appDbContext.BasketItems.FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

            if (basketItem == null)
                return NotFound();

            if(basketItem.Count > 1)
            {
                basketItem.Count--;

            }

            _appDbContext.Update(basketItem);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Basket");

        }

    }
}
