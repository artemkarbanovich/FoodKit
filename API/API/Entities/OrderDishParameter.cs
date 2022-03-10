namespace API.Entities;

public class OrderDishParameter
{
    public int Id { get; set; }
    public int NumberOfPersons { get; set; }
    public int Count { get; set; }

    public Dish Dish { get; set; }
    public int DishId { get; set; }
    public Order Order { get; set; }
    public int OrderId { get; set; }
}