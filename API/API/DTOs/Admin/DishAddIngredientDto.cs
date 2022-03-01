using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Admin;

public class DishAddIngredientDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [Range(0, 350)]
    public int IngredientWeightPerPortion { get; set; }
}