namespace Core.Common.Mailer;

internal class MailerMessage
{
    public required MailerAddress To { get; set; }
    public MailerAddress? From { get; set; }
    public MailerAddress? ReplyTo { get; set; }
    public required string Subject { get; set; }
    public required string BodyHtml { get; set; }
    public string? BodyPlain { get; set; }
}
