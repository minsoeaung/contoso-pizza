using ContosoPizza.Models;
using ContosoPizza.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ContosoPizza.Services;

public class MailService : IMailService
{
    private readonly MailConfig _mailConfig;

    public MailService(IOptions<MailConfig> mailConfig)
    {
        _mailConfig = mailConfig.Value;
    }

    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailConfig.Mail);
        email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
        email.Subject = mailRequest.Subject;

        var builder = new BodyBuilder();

        if (mailRequest.Attachments != null)
        {
            foreach (var file in mailRequest.Attachments.Where(file => file.Length > 0))
            {
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                }

                builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            }
        }

        builder.HtmlBody = mailRequest.Body;
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_mailConfig.Host, _mailConfig.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_mailConfig.Mail, _mailConfig.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}