using API.Helpers.Paginator;
using System.Text.Json;

namespace API.Extensions;

public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response,
        int currentPage, int pageSize, int totalCount)
    {
        var paginationHeader = new PaginationHeader(currentPage, pageSize, totalCount);
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }
}