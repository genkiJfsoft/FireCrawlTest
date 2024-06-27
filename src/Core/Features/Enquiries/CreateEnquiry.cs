using Ardalis.Result;
using Core.Providers.Mailer;
using Core.Utilities;
using MediatR;

namespace Core.Features.Enquiries;

public record CreateEnquiry(string Title = "Enquiry") : IRequest<Result>
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Subject { get; init; }
    public required string Message { get; init; }
}

internal class CreateEnquiryHandler(IMailer mailer) : IRequestHandler<CreateEnquiry, Result>
{
    public async Task<Result> Handle(CreateEnquiry request, CancellationToken cancellationToken)
    {
        try
        {
            var bodyHtml = AssetsHelper.ReadAsString("Assets/MailTemplates/Enquiry.htm");

            var message = new MailerMessage
            {
                To = new MailerAddress("Enquiry", "enquiry@example.com"),
                ReplyTo = new MailerAddress(request.Name, request.Email),
                Subject = $"[{request.Title}] {request.Subject}",
                BodyHtml = bodyHtml
                    .Replace("{Title}", request.Title)
                    .Replace("{Name}", request.Name)
                    .Replace("{Email}", request.Email)
                    .Replace("{Subject}", request.Subject)
                    .Replace("{Message}", request.Message),
                BodyPlain = request.Message
            };

            await mailer.SendEmailAsync(message);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}

