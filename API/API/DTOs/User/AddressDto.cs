using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User;

public class AddressDto
{
    public int? Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Country { get; set; }

    [Required]
    [MaxLength(50)]
    public string Locality { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Street { get; set; }

    [Required]
    [MaxLength(10)]
    public string HouseNumber { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string ApartmentNumber { get; set; }

    [MaxLength(5)]
    public string EntranceNumber { get; set; }

    [MaxLength(5)]
    public string Floor { get; set; }
}