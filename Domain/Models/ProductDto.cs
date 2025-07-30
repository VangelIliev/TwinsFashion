using Data.Models;

namespace Domain.Models
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public required string Description { get; set; }

        public Guid CategoryId { get; set; }

        public required Category Category { get; set; }

        public required string Size { get; set; }

        public Guid ColorId { get; set; }

        public required Color Color { get; set; }

        public ICollection<Image> Images { get; set; } = [];
    }
}
