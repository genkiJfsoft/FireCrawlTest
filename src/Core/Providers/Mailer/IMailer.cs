namespace Core.Providers.Mailer;

internal interface IMailer
{
    Task SendEmailAsync(MailerMessage message);
}
