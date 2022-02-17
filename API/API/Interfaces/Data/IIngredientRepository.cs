using API.DTOs.Admin;
using API.Entities;
using API.Helpers.Paginator;
using API.Helpers.QueryParams;

namespace API.Interfaces.Data;

public interface IIngredientRepository
{
    Task<bool> AnyIngredientsByLevenshteinDistanceAsync(int maxDistance, string name);
    Task AddIngredientAsync(Ingredient ingredient);
    Task<PagedList<IngredientDto>> GetIngredientsAsync(IngredientParam ingredientParam);
    Task<Ingredient> GetIngredientByIdAsync(int id);
    void DeleteIngredient(Ingredient ingredient);
}