using Microsoft.EntityFrameworkCore;
using Pronia.Abstractions;
using Pronia.Context;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pronia.Services
{
    public class BasketService(AppDbContext context, IHttpContextAccessor accessor) : IBasketService
    {
        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            string userId = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            var isExistUser = await context.Users.AnyAsync(x=>x.Id == userId);

            if (!isExistUser)
                return [];

            var basketItems = await context.BasketItems.Include(x=>x.Product).Where(x=>x.UserId == userId).ToListAsync();

            return basketItems;         

        }

    }
}
