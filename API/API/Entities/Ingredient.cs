namespace API.Entities;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Proteins { get; set; }         //per 100 grams
    public float Fats { get; set; }             //per 100 grams
    public float Carbohydrates { get; set; }    //per 100 grams
    public int Calories { get; set; }           //per 100 grams

    public ICollection<Dish> Dishes { get; set; }
}