using Ardalis.Result;
using Core.Providers.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Collections;

public record GetCollections : IRequest<Result<List<CollectionDto>>>
{
}

internal class GetCollectionsHandler(DataContext db) : IRequestHandler<GetCollections, Result<List<CollectionDto>>>
{
    public async Task<Result<List<CollectionDto>>> Handle(GetCollections request, CancellationToken cancellationToken)
    {
        var items = await db.Collections
            .AsNoTracking()
            .Select(e => new CollectionDto
            {
                Id = e.Id,
                PublicId = e.PublicId,
                Title = e.Title,
                Description = e.Description,
                CreatedAt = e.CreatedAt,
                LastModifiedAt = e.LastModifiedAt,
            })
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return items;
    }
}
