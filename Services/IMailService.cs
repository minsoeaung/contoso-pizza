using ContosoPizza.Models;

namespace ContosoPizza.Services;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}