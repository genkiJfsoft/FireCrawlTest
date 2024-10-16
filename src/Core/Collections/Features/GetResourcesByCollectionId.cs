using Ardalis.Result;
using Core.Collections.Data;
using Core.Common.Data;
using MediatR;

namespace Core.Collections.Features;

public record GetResourcesByCollectionId : IRequest<Result<PaginatedList<ResourceData>>>
{
    public required int CollectionId { get; init; }
    public int Page { get; init; } = 1;
    public required int PerPage { get; init; }
}

internal class GetResourcesByCollectionIdHandler(IDataContext db) : IRequestHandler<GetResourcesByCollectionId, Result<PaginatedList<ResourceData>>>
{
    public async Task<Result<PaginatedList<ResourceData>>> Handle(GetResourcesByCollectionId request, CancellationToken cancellationToken)
    {
        var list = await db.Resources
            .Where((e) => e.CollectionId == request.CollectionId).OrderByDescending((e) => e.CreatedAt)
            .Select(e => new ResourceData
            {
                Id = e.Id,
                CollectionId = e.CollectionId,
                Title = e.Title,
                Notes = e.Notes,
                LinkToUrl = e.LinkToUrl,
                CreatedAt = e.CreatedAt,
                LastModifiedAt = e.LastModifiedAt,
            })
            .ToPaginatedListAsync(request.Page, request.PerPage, cancellationToken);

        return Result<PaginatedList<ResourceData>>.Success(list);
    }
}
