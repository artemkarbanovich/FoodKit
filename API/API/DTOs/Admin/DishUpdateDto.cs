using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Admin;

public class DishUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 1)]
    public string Name { get; set; }

    [Required]
    [Range(0, 24)]
    public int CookingTimeHours { get; set; }

    [Required]
    [Range(0, 60)]
    public int CookingTimeMinutes { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string YouWillNeed { get; set; }

    [Required]
    [Range(0, 300)]
    public decimal Price { get; set; }

    [Required]
    public bool IsAvailableForSingleOrder { get; set; }
}