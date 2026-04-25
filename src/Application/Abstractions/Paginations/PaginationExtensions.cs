using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Abstractions.Paginations;

public static class PaginationExtensions
{
    public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(
        this IQueryable<T> queryable,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await queryable.CountAsync(cancellationToken);
        var items = await queryable
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResponse<T>(items, page, pageSize, totalCount, totalPages);
    }

    public static PagedResponse<T> ToPagedResponse<T>(
        this IEnumerable<T> source,
        int page,
        int pageSize)
    {
        var list = source.ToList();

        var totalCount = list.Count;

        var items = list
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResponse<T>(items, page, pageSize, totalCount, totalPages);
    }
}