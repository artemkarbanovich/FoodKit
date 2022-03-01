namespace API.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public decimal TotalPrice { get; set; }
    public int Evaluation { get; set; }

    public ICollection<OrderDishParameter> OrderDishParameters { get; set; }

    public AppUser AppUser { get; set; }
    public int AppUserId { get; set; }
}