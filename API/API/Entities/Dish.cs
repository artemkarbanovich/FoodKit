namespace API.Entities;

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TimeOnly CookingTime { get; set; }
    public string YouWillNeed { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailableForSingleOrder { get; set; }

    public ICollection<Image> Images { get; set; }
    public ICollection<DishIngredient> Ingredients { get; set; }
}