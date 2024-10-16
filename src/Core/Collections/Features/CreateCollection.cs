using Ardalis.Result;
using Core.Collections.Data;
using Core.Common.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Core.Collections.Features;

[Authorize]
public record CreateCollection : IRequest<Result>
{
    public required string Title { get; init; }
    public string Description { get; init; } = string.Empty;
}

internal class CreateCollectionHandler(IDataContext db) : IRequestHandler<CreateCollection, Result>
{
    public async Task<Result> Handle(CreateCollection request, CancellationToken cancellationToken)
    {
        // TODO: add validation

        var collection = new Collection { Title = request.Title, Description = request.Description };

        db.Collections.Add(collection);

        await db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
