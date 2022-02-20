using API.DTOs.Admin;
using API.Entities;
using API.Interfaces.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class DishRepository : IDishRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public DishRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }


    public async Task<int> AddDishAsync(DishAddDto dishAddDto)
    {
        using var transaction = await _dataContext.Database.BeginTransactionAsync();
        try
        {
            var dish = new Dish();
            var ingredients = new List<DishIngredient>();

            _mapper.Map(dishAddDto, dish);

            await _dataContext.Dishes.AddAsync(dish);

            if (!(await _dataContext.SaveChangesAsync() > 0)) 
                return -1;

            foreach(var i in dishAddDto.Ingredients)
            {
                ingredients.Add(new DishIngredient
                {
                    IngredientWeightPerPortion = i.IngredientWeightPerPortion,
                    DishId = dish.Id,
                    IngredientId = i.Id
                });
            }

            await _dataContext.DishIngredients.AddRangeAsync(ingredients);

            if (!(await _dataContext.SaveChangesAsync() > 0)) 
                return -1;

            await transaction.CommitAsync();
            return dish.Id;
        }
        catch(Exception)
        {
            await transaction.RollbackAsync();
            return -1;
        }
    }

    public async Task AddImageAsync(Image image)
    {
        await _dataContext.Images.AddAsync(image);
    }

    public async Task<Image> GetImageByIdAsync(int imageId)
    {
        return await _dataContext.Images.FirstOrDefaultAsync(i => i.Id == imageId);
    }

    public void RemoveImage(Image image)
    {
        _dataContext.Images.Remove(image);
    }
}