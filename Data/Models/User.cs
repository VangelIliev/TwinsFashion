
namespace Data.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public  string AddressForOrder { get; set; }
    }
}
