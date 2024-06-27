using Ardalis.Result;
using Core.Entities;
using Core.Providers.Data;
using MediatR;

namespace Core.Features.Collections;

public record CreateCollection : IRequest<Result>
{
    public required string Title { get; init; }
    public string Description { get; init; } = string.Empty;
}

internal class CreateCollectionHandler(DataContext db) : IRequestHandler<CreateCollection, Result>
{
    public async Task<Result> Handle(CreateCollection request, CancellationToken cancellationToken)
    {
        // TODO: add validation

        var collection = new Collection { Title = request.Title, Description = request.Description };

        db.Add(collection);

        await db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
