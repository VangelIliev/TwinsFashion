using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }
        public DateTime Date { get; set; }

        public required ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
