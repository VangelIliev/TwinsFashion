using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Color
    {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; }
    }
}
