using ThoughtEcho.Models;

namespace ThoughtEcho.Services.Interfaces
{
    public interface IEmailService
    {
        bool SendEmail(EmailModel request);
    }
}
