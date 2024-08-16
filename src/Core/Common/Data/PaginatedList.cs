using Microsoft.EntityFrameworkCore;

namespace Core.Common.Data;

public class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    public PaginatedList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = pageSize > 0 ? (int)Math.Ceiling(count / (double)pageSize) : 1;
        TotalCount = count;
        Items = items;
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> FromQueryAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        // Apply pagination query only if pageSize > 0. Otherwise, skip it.

        var itemsQuery = pageSize > 0 ? source.Skip((pageNumber - 1) * pageSize).Take(pageSize) : source;
        var items = await itemsQuery.ToListAsync(cancellationToken);
        var count = pageSize > 0 ? await source.CountAsync(cancellationToken) : items.Count;

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}
