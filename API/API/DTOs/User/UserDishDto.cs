using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User;

public class UserDishDto
{
    public int? Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; }

    [Required]
    public DateOnly DishDate { get; set; }
    
    [Required]
    [RegularExpression("^[1-9][0-9]*$")]
    [Range(1, 5000)]
    public int DishWeight { get; set; }

    [Required]
    [RegularExpression("^\\d*\\.?\\d*$")]
    [Range(0, 200)]
    public float Proteins { get; set; }

    [Required]
    [RegularExpression("^\\d*\\.?\\d*$")]
    [Range(0, 200)]
    public float Fats { get; set; }
    
    [Required]
    [RegularExpression("^\\d*\\.?\\d*$")]
    [Range(0, 200)]
    public float Carbohydrates { get; set; }
    
    [Required]
    [RegularExpression("^[1-9][0-9]*$")]
    [Range(1, 1100)]
    public int Calories { get; set; }
}