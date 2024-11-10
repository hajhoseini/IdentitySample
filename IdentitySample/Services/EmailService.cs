using System.Net;
using System.Net.Mail;
using System.Text;

namespace IdentitySample.Services
{
    public class EmailService
    {
        private string mailAddress = "bageto.net@gmail.com";
        private string password = "123";

        public Task Execute(string email, string subject, string body)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587; //gmail
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 1000000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(mailAddress, password);

            MailMessage message = new MailMessage(mailAddress, email, subject, body);
            message.IsBodyHtml = true;
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

            client.Send(message);

            return Task.CompletedTask;
        }

        //https://myaccount.google.com/lesssecureapps 
        //به لینک فوق می رویم و در تنظیمات جیمیل مان گزینه
        // اجازه به برنامه های با امنیت کمتر را فعال می کنیم
    }
}
