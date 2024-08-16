using Ardalis.Result;
using Core.Collections.Data;
using Core.Common.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Collections.Features;

public record GetCollection(string PublicId) : IRequest<Result<CollectionData>>;

internal class GetCollectionHandler(DataContext db) : IRequestHandler<GetCollection, Result<CollectionData>>
{
    public async Task<Result<CollectionData>> Handle(GetCollection request, CancellationToken cancellationToken)
    {
        var item = await db.Collections
            .AsNoTracking()
            .Where(e => e.PublicId == request.PublicId)
            .Select(e => new CollectionData
            {
                Id = e.Id,
                PublicId = e.PublicId,
                Title = e.Title,
                Description = e.Description,
                CreatedAt = e.CreatedAt,
                LastModifiedAt = e.LastModifiedAt,
            })
            .FirstOrDefaultAsync(cancellationToken);

        return item != null ? Result<CollectionData>.Success(item) : Result<CollectionData>.NotFound();
    }
}
