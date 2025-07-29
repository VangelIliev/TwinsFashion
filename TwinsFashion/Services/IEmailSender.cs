using System.Net.Http.Headers;
using TwinsFashion.Models;

namespace TwinsFashion.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailViewModel model, IEnumerable<ProductViewModel> products);

        Task SendContactUsEmail(EmailContactUsViewModel model);
    }
}