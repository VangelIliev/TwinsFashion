using System.ComponentModel.DataAnnotations;

namespace TwinsFashion.Models
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
