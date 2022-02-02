namespace API.Entities;

public class Address
{
    public int Id { get; set; }
    public string Country { get; set; }
    public string Locality { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string ApartmentNumber { get; set; }
    public string EntranceNumber { get; set; }
    public string Floor { get; set; }

    public AppUser AppUser { get; set; }
    public int AppUserId { get; set; }
}