namespace API.Entities;

public class OrderSetParameter
{
    public int Id { get; set; }
    public int NumberOfMeals { get; set; }
    public int NumberOfPersons { get; set; }

    public Set Set { get; set; }
    public int SetId { get; set; }
}