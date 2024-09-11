using Ardalis.Result;
using Core.Collections.Data;
using Core.Common.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Core.Collections.Features;

[Authorize]
public record UpdateCollection : IRequest<Result>
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public string Description { get; init; } = string.Empty;
}

internal class UpdateCollectionHandler(DataContext db) : IRequestHandler<UpdateCollection, Result>
{
    public async Task<Result> Handle(UpdateCollection request, CancellationToken cancellationToken)
    {
        var collection = db.Collections.FirstOrDefault(e => e.Id == request.Id);

        if (collection == null)
        {
            return Result.Conflict();
        }

        collection.Title = request.Title;
        collection.Description = request.Description;

        await db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
