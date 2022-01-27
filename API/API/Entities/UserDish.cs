namespace API.Entities;

public class UserDish
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly DishDate { get; set; }
    public int DishWeight { get; set; }         //gram
    public float Proteins { get; set; }         //per 100 grams
    public float Fats { get; set; }             //per 100 grams
    public float Carbohydrates { get; set; }    //per 100 grams
    public int Calories { get; set; }           //per 100 grams

    public AppUser AppUser { get; set; }
    public int AppUserId { get; set; }
}