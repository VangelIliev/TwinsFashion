namespace TwinsFashion.Models
{
    public class ShoppingBasketViewModel
    {
        public ShoppingBasketViewModel()
        {
            this.Products = new Dictionary<Guid, List<ProductViewModel>>();
            this.EmailModel = new EmailViewModel();
        }
        public Dictionary<Guid, List<ProductViewModel>> Products { get; set; }
        public EmailViewModel EmailModel { get; set; }
    }
}
