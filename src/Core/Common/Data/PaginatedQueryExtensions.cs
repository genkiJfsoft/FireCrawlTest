using Microsoft.EntityFrameworkCore;

namespace Core.Common.Data;

public static class PaginatedQueryExtensions
{
    /// <summary>
    /// Applies pagination to the source queryable based on the provided page information.
    /// If the page size is greater than 0, it applies offset pagination to the query.
    /// Otherwise, it skips the pagination.
    /// </summary>
    /// <typeparam name="TDestination">The type of the elements in the source queryable.</typeparam>
    /// <param name="queryable">The queryable to apply pagination to</param>
    /// <param name="pageNumber">The page index number</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize, CancellationToken cancellationToken = default) where TDestination : class
        => PaginatedList<TDestination>.FromQueryAsync(queryable.AsNoTracking(), pageNumber, pageSize, cancellationToken);
}
