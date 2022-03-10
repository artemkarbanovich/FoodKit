namespace API.Helpers.QueryParams;

public class DishUserListParam
{
    private const int MinPageSize = 5;
    private const int MaxPageSize = 10;
    private int pageSize = 5;

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