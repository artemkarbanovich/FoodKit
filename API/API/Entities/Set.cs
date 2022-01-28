namespace API.Entities;

public class Set
{
    public int Id { get; set; }
    public string PrivateName { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public float Price { get; set; }
    public DateTime DateAdded { get; set; }
    public bool IsAvailableForOrder { get; set; }

    public ICollection<Dish> Dishes { get; set; }
}