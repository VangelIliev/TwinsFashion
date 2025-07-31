using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class SubCategory
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
