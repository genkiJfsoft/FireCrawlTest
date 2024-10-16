using Ardalis.Result;
using Core.Collections.Data;
using Core.Common.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Core.Collections.Features;

[Authorize]
public record CreateResource : IRequest<Result>
{
    public required int CollectionId { get; init; }
    public required string Title { get; init; }
    public string? Notes { get; init; }
    public required string LinkToUrl { get; init; }
}

internal class CreateResourceHandler(IDataContext db) : IRequestHandler<CreateResource, Result>
{
    public async Task<Result> Handle(CreateResource request, CancellationToken cancellationToken)
    {
        // TODO: add validation

        try
        {
            var resource = new Resource
            {
                CollectionId = request.CollectionId,
                Title = request.Title,
                Notes = request.Notes,
                LinkToUrl = request.LinkToUrl
            };

            db.Resources.Add(resource);

            await db.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
