using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.Shared.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace ChinarAz.Infrastructure.Services;

public class EmailService : IEmailService
{
    private EmailSettings _settings { get; }

    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendEmailAsync(IEnumerable<string> toEmails, string subject, string body)
    {
        var mail = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        foreach (var email in toEmails)
        {
            if (string.IsNullOrWhiteSpace(email))
                continue; // və ya throw new ArgumentException("Invalid email.");

            mail.To.Add(email);
        }

        using var smtp = new SmtpClient(_settings.SmtpServer, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password),
            EnableSsl = true
        };

        await smtp.SendMailAsync(mail);
    }
}
