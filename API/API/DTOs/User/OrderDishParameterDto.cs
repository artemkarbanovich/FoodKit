using API.Helpers;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User;

public class OrderDishParameterDto
{
    public int Id { get; set; }

    [Required]
    public int DishId { get; set; }

    [Required]
    [DishNumPersons]
    public int NumberOfPersons { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Count { get; set; }
}