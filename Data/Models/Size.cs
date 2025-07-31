using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Size
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
