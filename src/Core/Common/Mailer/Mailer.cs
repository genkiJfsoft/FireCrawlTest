using Core.Common.Configurations;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Core.Common.Mailer;

internal class Mailer : IMailer
{
    private readonly MailerConfiguration _config;

    public Mailer(IConfiguration configuration)
    {
        _config = configuration.GetSection<MailerConfiguration>(MailerConfiguration.Key);
    }

    public async Task SendEmailAsync(MailerMessage message)
    {
        var mail = new MimeMessage();
        mail.From.Add(message.From?.ToMailboxAddress() ?? _config.MailFrom.ToMailboxAddress());
        mail.To.Add(message.To.ToMailboxAddress());
        if (message.ReplyTo != null)
        {
            mail.ReplyTo.Add(message.ReplyTo.ToMailboxAddress());
        }
        mail.Subject = message.Subject;
        mail.Body = new BodyBuilder
        {
            HtmlBody = message.BodyHtml,
            TextBody = message.BodyPlain ?? message.Subject
        }.ToMessageBody();

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_config.SmtpServer, _config.SmtpPort, false);
            if (_config.SmtpUsername != null && _config.SmtpUsername != null)
            {
                await client.AuthenticateAsync(_config.SmtpUsername, _config.SmtpPassword);
            }
            await client.SendAsync(mail);
            await client.DisconnectAsync(true);
        }
    }
}
