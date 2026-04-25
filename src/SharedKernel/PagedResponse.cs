namespace SharedKernel;

public sealed record PagedResponse<T>(
    List<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages)
{
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;

    public static PagedResponse<T> Empty(int pageSize = 10) =>
        new([], 1, pageSize, 0, 0);
}