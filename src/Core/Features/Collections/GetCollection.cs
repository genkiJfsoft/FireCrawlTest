using Ardalis.Result;
using Core.Providers.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Collections;

public record GetCollection(string PublicId) : IRequest<Result<CollectionDto>>;

internal class GetCollectionHandler(DataContext db) : IRequestHandler<GetCollection, Result<CollectionDto>>
{
    public async Task<Result<CollectionDto>> Handle(GetCollection request, CancellationToken cancellationToken)
    {
        var item = await db.Collections
            .AsNoTracking()
            .Where(e => e.PublicId == request.PublicId)
            .Include("Resources")
            .Select(e => new CollectionDto
            {
                Id = e.Id,
                PublicId = e.PublicId,
                Title = e.Title,
                Description = e.Description,
                Resources = e.Resources.Select(r => new ResourceDto
                {
                    Id = r.Id,
                    CollectionId = r.CollectionId,
                    Title = r.Title,
                    Notes = r.Notes,
                    LinkToUrl = r.LinkToUrl,
                    CreatedAt = r.CreatedAt,
                    LastModifiedAt = r.LastModifiedAt,
                }).ToList(),
                CreatedAt = e.CreatedAt,
                LastModifiedAt = e.LastModifiedAt,
            })
            .FirstOrDefaultAsync();

        return item != null ? Result<CollectionDto>.Success(item) : Result<CollectionDto>.NotFound();
    }
}
