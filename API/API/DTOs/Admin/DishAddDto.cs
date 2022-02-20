using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Admin;

public class DishAddDto
{
    [StringLength(30, MinimumLength = 1)]
    public string Name { get; set; }

    [Required]
    public TimeOnly CookingTime { get; set; }

    [StringLength(100, MinimumLength = 5)]
    public string YouWillNeed { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal Price { get; set; }

    [Required]
    public bool IsAvailableForSingleOrder { get; set; }

    [Required]
    public ICollection<DishAddIngredientDto> Ingredients { get; set; }
}