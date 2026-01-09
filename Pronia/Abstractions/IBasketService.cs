namespace Pronia.Abstractions
{
    public interface IBasketService
    {
        Task<List<BasketItem>> GetBasketItemsAsync();
    }
}
