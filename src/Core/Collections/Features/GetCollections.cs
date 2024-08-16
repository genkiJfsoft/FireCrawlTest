using Ardalis.Result;
using Core.Collections.Data;
using Core.Common.Data;
using MediatR;

namespace Core.Collections.Features;

public record GetCollections : IRequest<Result<PaginatedList<CollectionData>>>
{
    public int Page { get; init; } = 1;

    public required int PerPage { get; init; }
}

internal class GetCollectionsHandler(DataContext db) : IRequestHandler<GetCollections, Result<PaginatedList<CollectionData>>>
{
    public async Task<Result<PaginatedList<CollectionData>>> Handle(GetCollections request, CancellationToken cancellationToken)
    {
        var list = await db.Collections
            .OrderByDescending((e) => e.CreatedAt)
            .Select(e => new CollectionData // TODO: map objects using AutoMapper
            {
                Id = e.Id,
                PublicId = e.PublicId,
                Title = e.Title,
                Description = e.Description,
                CreatedAt = e.CreatedAt,
                LastModifiedAt = e.LastModifiedAt,
            })
            .ToPaginatedListAsync(request.Page, request.PerPage, cancellationToken);

        return Result<PaginatedList<CollectionData>>.Success(list);
    }
}
