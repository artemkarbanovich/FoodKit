namespace API.Entities;

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TimeOnly CookingTime { get; set; }
    public string YouWillNeed { get; set; }
    public int DishWeight { get; set; }
    public float Price { get; set; }
    public bool IsAvailableForSingleOrder { get; set; }

    public ICollection<Image> Images { get; set; }
    public ICollection<Set> Sets { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; }
}