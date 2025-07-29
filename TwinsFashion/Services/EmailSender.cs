using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;
using TwinsFashion.Models;

namespace TwinsFashion.Services
{
    public class EmailSender : IEmailSender
    {
        // Our private configuration variables
        private string host;
        private int port;
        private bool enableSSL;
        private string userName;
        private string password;
        public EmailSender(IOptions<EmailSettings> options)
        {
            var settings = options.Value;
            host = settings.Host;
            port = settings.Port;
            enableSSL = settings.EnableSSL;
            userName = settings.UserName;
            password = settings.Password;
        }
        public Task SendEmailAsync(EmailViewModel model, IEnumerable<ProductViewModel> products)
        {
            var client = new SmtpClient(host, port)
            {
                EnableSsl = enableSSL,
                Credentials = new NetworkCredential(userName, password)
            };

            var body = PopulateBody(model, products);

            return client.SendMailAsync(
                new MailMessage(userName, userName, "Поръчка на продукти", body)
                { IsBodyHtml = true });
        }
        public Task SendContactUsEmail(EmailContactUsViewModel model)
        {
            var client = new SmtpClient(host, port)
            {
                EnableSsl = enableSSL,
                Credentials = new NetworkCredential(userName, password)
            };

            return client.SendMailAsync(new MailMessage(model.EmailAddress, userName, "Въпроси :", model.Description));
        }
        private string PopulateBody(EmailViewModel model, IEnumerable<ProductViewModel> products)
        {
            var stringBuilder = new StringBuilder();

            if (products.Any())
            {
                stringBuilder.AppendLine($" <br> <br> <b>Поръчани продукти:</b> <br>");
                foreach (var product in products)
                {
                    stringBuilder.AppendLine($"Име на продукт: {product.Name} <br>");
                    stringBuilder.AppendLine($"Тип на продукт : {product.Category}, Цвят {product.Color}, размер {product.Size}, Цена {product.Price}");
                }

                stringBuilder.AppendLine($" <br> <br> <b>Обща сума на поръчката:</b> {products.Sum(p => p.Price)} лв.");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
