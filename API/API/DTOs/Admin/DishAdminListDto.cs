namespace API.DTOs.Admin;

public class DishAdminListDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailableForSingleOrder { get; set; }
    public string ImagePath { get; set; }
}