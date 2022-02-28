using API.DTOs.Admin;

namespace API.DTOs;

public class DishDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CookingTimeHours { get; set; }
    public int CookingTimeMinutes { get; set; }
    public string YouWillNeed { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailableForSingleOrder { get; set; }

    public ICollection<ImageDto> Images { get; set; }
    public ICollection<IngredientDto> Ingredients { get; set; }
}