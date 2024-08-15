using Ardalis.Result;
using Core.Collections.Data;
using Core.Common.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Collections.Features;

public record GetCollections : IRequest<PagedResult<List<CollectionData>>>
{
    public int Page { get; init; } = 1;

    public required int PerPage { get; init; }
}

internal class GetCollectionsHandler(DataContext db) : IRequestHandler<GetCollections, PagedResult<List<CollectionData>>>
{
    public async Task<PagedResult<List<CollectionData>>> Handle(GetCollections request, CancellationToken cancellationToken)
    {
        PagedInfo pagedInfo = new(request.Page, request.PerPage, 0, 0);

        var query = db.Collections.AsNoTracking();

        var items = await query
            .MaybePaginate(pagedInfo) // apply pagination if pagedInfo.PageSize > 0
            .OrderBy(e => e.CreatedAt)
            .Select(e => new CollectionData
            {
                Id = e.Id,
                PublicId = e.PublicId,
                Title = e.Title,
                Description = e.Description,
                CreatedAt = e.CreatedAt,
                LastModifiedAt = e.LastModifiedAt,
            })
            .ToListAsync(cancellationToken);

        // If pageInfo.PageSize > 0, MaybePaginate() will apply pagination to the items query.
        // To avoid unnecessary load, we fetch the total records from total count query
        // only when the pageInfo.PageSize > 0. Otherwise, get the total records from items.Count.
        pagedInfo.SetTotal(pagedInfo.PageSize > 0 ? await query.CountAsync(cancellationToken) : items.Count);

        return Result<List<CollectionData>>.Success(items).ToPagedResult(pagedInfo);
    }
}
