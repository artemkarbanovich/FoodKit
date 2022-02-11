using Microsoft.EntityFrameworkCore;

namespace API.Helpers.Paginator;

public class PagedList<T> : List<T>
{
    public PagedList(IEnumerable<T> items, int currentPage, int pageSize, int totalCount)
    {
        AddRange(items);
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int currentPage, int pageSize)
    {
        var totalCount = await source.CountAsync();
        var items = await source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedList<T>(items, currentPage, pageSize, totalCount);
    }
}