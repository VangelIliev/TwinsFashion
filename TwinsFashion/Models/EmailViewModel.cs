using System.ComponentModel.DataAnnotations;

namespace TwinsFashion.Models
{
    public class EmailViewModel
    {
        [Required(ErrorMessage = "Полето е задължително")]
        public  string FirstAndLastName { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [RegularExpression("08[789]\\d{7}", ErrorMessage = "Невалиден телефонен номер")]
        public  string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public  string City { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public string ShippingMethod { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public  string Address { get; set; }

        public string? Comment { get; set; }
    }
}
