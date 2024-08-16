using Core.Common.Mailer;
using Core.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Core.Identity.Services;

internal sealed class IdentityMailer : IEmailSender<User>
{
    private readonly IMailer _mailer;

    public IdentityMailer(IMailer mailer)
    {
        _mailer = mailer;
    }

    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink) =>
        _mailer.SendEmailAsync(new MailerMessage()
        {
            To = new MailerAddress(user.UserName ?? "User", email),
            Subject = "Confirm your emai",
            BodyHtml = $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>."
        });

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink) =>
        _mailer.SendEmailAsync(new MailerMessage()
        {
            To = new MailerAddress(user.UserName ?? "User", email),
            Subject = "Reset your password",
            BodyHtml = $"Please reset your password by <a href='{resetLink}'>clicking here</a>."
        });

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode) =>
        _mailer.SendEmailAsync(new MailerMessage()
        {
            To = new MailerAddress(user.UserName ?? "User", email),
            Subject = "Reset your password",
            BodyHtml = $"Please reset your password using the following code: {resetCode}"
        });
}
