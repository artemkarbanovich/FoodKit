using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Admin;

public class IngredientDto
{
    public int? Id { get; set; }

    [Required]
    [MaxLength(25)]
    public string Name { get; set; }

    [Required]
    [Range(0, 200)]
    public float Proteins { get; set; }

    [Required]
    [Range(0, 200)]
    public float Fats { get; set; }

    [Required]
    [Range(0, 200)]
    public float Carbohydrates { get; set; }

    [Required]
    [RegularExpression("^[1-9][0-9]*$")]
    [Range(1, 1100)]
    public int Calories { get; set; }
}