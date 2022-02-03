using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    [RegularExpression("^(\\+375)(29|25|44|33)(\\d{3})(\\d{2})(\\d{2})$")]
    public string PhoneNumber { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 6)]
    public string Password { get; set; }
}