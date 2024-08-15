using Ardalis.Result;

namespace Core.Common.Data;

internal static class PagedExtensions
{

    public static PagedInfo SetTotal(this PagedInfo pageInfo, long totalRecords)
    {
        var totalPages = pageInfo.PageSize > 0 ? (long)Math.Ceiling((decimal)totalRecords / pageInfo.PageSize) : 0;

        pageInfo.SetTotalRecords(totalRecords);
        pageInfo.SetTotalPages(totalPages);

        return pageInfo;
    }

    /// <summary>
    /// Applies pagination to the source queryable based on the provided paged information.
    /// If the page size is greater than 0, it skips the appropriate number of records and takes the specified page size.
    /// Otherwise, it returns the source queryable without any pagination applied.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source queryable.</typeparam>
    /// <param name="source">The queryable to apply pagination to.</param>
    /// <param name="pagedInfo">The paged information containing the page number and page size.</param>
    /// <returns>The queryable with pagination applied if the page size is greater than 0, otherwise the source queryable.</returns>
    public static IQueryable<TSource> MaybePaginate<TSource>(this IQueryable<TSource> source, PagedInfo pagedInfo)
    {
        return pagedInfo.PageSize > 0
            ? source.Skip((int)((pagedInfo.PageNumber - 1) * pagedInfo.PageSize)).Take((int)pagedInfo.PageSize)
            : source;
    }
}
