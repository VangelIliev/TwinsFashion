using System.ComponentModel.DataAnnotations;

namespace TwinsFashion.Models
{
    public class AdminUserViewModel
    {
        [Required]
        [MinLength(10)]
        public string Username { get; set; }

        [Required]
        [MinLength(10)]
        public string Password { get; set; }
    }
}
