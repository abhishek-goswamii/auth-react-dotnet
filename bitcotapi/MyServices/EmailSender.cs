using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace MyServices
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "bitcottest@outlook.com";
            var pw = "bitcot1234";

            using (var client = new SmtpClient("smtp-mail.outlook.com", 587))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(mail, pw);

                var mailMessage = new MailMessage(from: mail, to: email, subject, message);

                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    
                Console.WriteLine(ex.Message);

                }
            }
        }

        
    }
}
