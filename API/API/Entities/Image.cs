namespace API.Entities;

public class Image
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string PublicId { get; set; }

    public Dish Dish { get; set; }
    public int DishId { get; set; }
}