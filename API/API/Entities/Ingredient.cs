namespace API.Entities;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Proteins { get; set; }
    public float Fats { get; set; }
    public float Carbohydrates { get; set; }
    public int Calories { get; set; }

    public ICollection<Dish> Dishes { get; set; }
}