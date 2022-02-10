namespace API.Helpers.Paginator;

public class PaginationHeader
{
    public PaginationHeader(int currentPage, int pageSize, int totalCount, int totalPages)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }

    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}