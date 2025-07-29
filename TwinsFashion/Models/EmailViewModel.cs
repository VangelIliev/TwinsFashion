using System.ComponentModel.DataAnnotations;

namespace TwinsFashion.Models
{
    public class EmailViewModel
    {
        [Required(ErrorMessage = "Полето е задължително")]
        public required string FirstAndLastName { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [RegularExpression("08[789]\\d{7}", ErrorMessage = "Невалиден телефонен номер")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public required string City { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public required string Address { get; set; }

        public string? Comment { get; set; }
    }
}
