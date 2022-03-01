namespace API.Helpers.QueryParams;

public class DishAdminListParam
{
    private const int MinPageSize = 20;
    private const int MaxPageSize = 70;
    private int pageSize = 20;

    public int CurrentPage { get; set; } = 1;
    public int PageSize
    {
        get => pageSize;
        set
        {
            if (value < MinPageSize)
                pageSize = MinPageSize;
            else if (value > MaxPageSize)
                pageSize = MaxPageSize;
            else
                pageSize = value;
        }
    }
}