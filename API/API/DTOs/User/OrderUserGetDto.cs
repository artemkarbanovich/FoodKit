namespace API.DTOs.User;

public class OrderUserGetDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public decimal TotalPrice { get; set; }
    public int? Evaluation { get; set; }
    public string Status { get; set; }
}