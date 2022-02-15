using API.DTOs.Admin;
using API.Entities;
using API.Helpers.QueryParams;
using API.Helpers.Paginator;
using API.Interfaces.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using API.Extensions;

namespace API.Data.Repositories;

public class IngredientRepository : IIngredientRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public IngredientRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }


    public async Task AddIngredientAsync(Ingredient ingredient)
    {
        await _dataContext.Ingredients.AddAsync(ingredient);
    }

    public async Task<bool> AnyIngredientsByLevenshteinDistanceAsync(int maxDistance, string name)
    {
        var ingredients = await _dataContext.Ingredients
            .Select(i => i.Name)
            .ToListAsync();

        return ingredients.Any(i => i.LevenshteinDistance(name) <= maxDistance);
    }

    public async Task<PagedList<IngredientDto>> GetIngredientsAsync(IngredientParam ingredientParam)
    {
        var query = _dataContext.Ingredients.AsQueryable();
        var source = query.ProjectTo<IngredientDto>(_mapper.ConfigurationProvider).AsNoTracking();

        return await PagedList<IngredientDto>
           .CreateAsync(source, ingredientParam.CurrentPage, ingredientParam.PageSize);
    }

    public async Task<Ingredient> GetIngredientByIdAsync(int id)
    {
        return await _dataContext.Ingredients.SingleOrDefaultAsync(i => i.Id == id);
    }

    public void DeleteIngredient(Ingredient ingredient)
    {
        _dataContext.Ingredients.Remove(ingredient);
    }
}