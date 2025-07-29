namespace TwinsFashion.Models
{
    public class ShoppingBasketViewModel
    {
        public ShoppingBasketViewModel()
        {
            this.Products = [];
        }
        public List<ProductViewModel> Products { get; set; }
    }
}
