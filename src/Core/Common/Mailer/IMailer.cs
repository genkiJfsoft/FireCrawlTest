namespace Core.Common.Mailer;

internal interface IMailer
{
    Task SendEmailAsync(MailerMessage message);
}
