namespace Pronia.ViewModels.ProductViewModels
{
    public class ProductGetVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string MainImagePath { get; set; }
        public string HoverImagePath { get; set; }
        public double? Rating { get; set; }
        public List<string> TagNames { get; set; }

    }
}
