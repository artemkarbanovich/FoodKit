using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User;

public class UserDishDto
{
    public int? Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; }

    [Required] public DateOnly DishDate { get; set; }
    [Required] public int DishWeight { get; set; }
    [Required] public float Proteins { get; set; }
    [Required] public float Fats { get; set; }
    [Required] public float Carbohydrates { get; set; }
    [Required] public int Calories { get; set; }
}