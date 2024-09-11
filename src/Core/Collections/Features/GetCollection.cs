using Ardalis.Result;
using Core.Collections.Data;
using Core.Common.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Collections.Features;

public record GetCollection(string PublicId, bool IncludeResources = false) : IRequest<Result<CollectionData>>;

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
                Resources = request.IncludeResources ? e.Resources.Select(r => new ResourceData
                {
                    Id = r.Id,
                    CollectionId = r.CollectionId,
                    Title = r.Title,
                    Notes = r.Notes,
                    LinkToUrl = r.LinkToUrl,
                    CreatedAt = r.CreatedAt,
                    LastModifiedAt = r.LastModifiedAt,
                }).ToList() : new List<ResourceData>(),
                CreatedAt = e.CreatedAt,
                LastModifiedAt = e.LastModifiedAt,
            })
            .FirstOrDefaultAsync(cancellationToken);

        return item != null ? Result<CollectionData>.Success(item) : Result<CollectionData>.NotFound();
    }
}
