using MimeKit;

namespace Core.Common.Mailer;

internal record MailerAddress(string Name, string Address)
{
    public override string ToString() => $"{Name} <{Address}>";
    public MailboxAddress ToMailboxAddress() => new(Name, Address);
}
