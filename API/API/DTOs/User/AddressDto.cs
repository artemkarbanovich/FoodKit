using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User;

public class AddressDto
{
    public int? Id { get; set; }
    [Required] public string Country { get; set; }
    [Required] public string Locality { get; set; }
    [Required] public string Street { get; set; }
    [Required] public string HouseNumber { get; set; }
    [Required] public string ApartmentNumber { get; set; }
    public string EntranceNumber { get; set; }
    public string Floor { get; set; }
}