﻿namespace API.Helpers;

public class UserDishParams
{
    private const int MinPageSize = 10;
    private const int MaxPageSize = 50;
    private int pageSize = 10;

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
    public string SearchStringByName { get; set; } = "";
    public bool SortDishDateDescending { get; set; } = true;
}