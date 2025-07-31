using System.ComponentModel.DataAnnotations;

namespace TwinsFashion.Models
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            this.ImageUrls = [];    
        }
        
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required string Category { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
