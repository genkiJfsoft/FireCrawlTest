using Ardalis.Result;
using Core.Collections.Data;
using Core.Common.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Core.Collections.Features;

[Authorize]
public record DeleteCollection : IRequest<Result>
{
    public int? Id { get; init; }
    public string? PublicId { get; init; }

    public DeleteCollection(int id)
    {
        Id = id;
        PublicId = null;
    }

    public DeleteCollection(string publicId)
    {
        Id = null;
        PublicId = publicId;
    }
}

internal class DeleteCollectionHandler(IDataContext db) : IRequestHandler<DeleteCollection, Result>
{
    public async Task<Result> Handle(DeleteCollection request, CancellationToken cancellationToken)
    {
        // this will also deletes the Resources. See: https://learn.microsoft.com/en-us/ef/core/saving/cascade-delete
        var collection = db.Collections.Include(e => e.Resources)
            .FirstOrDefault(e => request.Id != null ? e.Id == request.Id : e.PublicId == request.PublicId! );

        if (collection == null)
        {
            return Result.Conflict();
        }

        db.Collections.Remove(collection);

        await db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
