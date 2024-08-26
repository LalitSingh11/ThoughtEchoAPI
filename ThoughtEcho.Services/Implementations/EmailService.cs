using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using ThoughtEcho.Models;
using ThoughtEcho.Services.Interfaces;
using ThoughtEcho.Utilities;

namespace ThoughtEcho.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public bool SendEmail(EmailModel request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(Config.GetMailSettings("Mail")));
            email.To.Add(MailboxAddress.Parse(request.SendTo));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(Config.GetMailSettings("Host"), int.Parse(Config.GetMailSettings("Port")), SecureSocketOptions.StartTls);
            smtp.Authenticate(Config.GetMailSettings("Mail"), Config.GetMailSettings("Password"));
            smtp.Send(email);
            smtp.Disconnect(true);
            return true;
        }
    }
}
