namespace Core.Common.Mailer;

internal record MailerConfiguration
{
    public const string Key = "Mailer";
    public required string SmtpServer { get; init; }
    public required int SmtpPort { get; init; }
    public string? SmtpUsername { get; init; }
    public string? SmtpPassword { get; init; }
    public required MailerAddress MailFrom { get; init; }
}
