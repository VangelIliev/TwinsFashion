using System.ComponentModel.DataAnnotations;


namespace Data.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [MinLength(5)]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Range(0, 1000)]
        public int Price { get; set; }

        [Range(0, 1000)]
        public int Quantity { get; set; }

        [MinLength(10)]
        [MaxLength(200)]
        public required string Description { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }

        public Guid SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public Guid ColorId { get; set; }

        public  Color Color { get; set; }
        public ICollection<Image> Images { get; set; } = [];

        public ICollection<OrderProduct> OrderProducts { get; set; } = [];

        public ICollection<Size> Sizes { get; set; } = [];
    }
}
