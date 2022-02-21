using API.DTOs.Admin;
using API.Entities;
using API.Interfaces;
using API.Interfaces.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class DishRepository : IDishRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly IImageService _imageService;

    public DishRepository(DataContext dataContext, IMapper mapper, IImageService imageService)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _imageService = imageService;
    }


    public async Task<bool> AddDishAsync(DishAddDto dishAddDto)
    {
        using var transaction = await _dataContext.Database.BeginTransactionAsync();
        try
        {
            var dish = new Dish();
            var ingredients = new List<DishIngredient>();
            var images = new List<Image>();

            await _dataContext.Dishes.AddAsync(_mapper.Map(dishAddDto, dish));

            if (!(await _dataContext.SaveChangesAsync() > 0)) 
            {
                await transaction.RollbackAsync();
                return false;
            }

            foreach(var ingr in dishAddDto.Ingredients)
            {
                ingredients.Add(new DishIngredient
                {
                    IngredientWeightPerPortion = ingr.IngredientWeightPerPortion,
                    DishId = dish.Id,
                    IngredientId = ingr.Id
                });
            }

            await _dataContext.DishIngredients.AddRangeAsync(ingredients);

            if (!(await _dataContext.SaveChangesAsync() > 0))
            {
                await transaction.RollbackAsync();
                return false;
            }

            foreach(var img in dishAddDto.Images)
            {
                var result = await _imageService.AddImageAsync(img);

                if (result.Error != null) return false;

                images.Add(new Image
                {
                    DishId = dish.Id,
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId
                });
            }

            await _dataContext.Images.AddRangeAsync(images);

            if (!(await _dataContext.SaveChangesAsync() > 0))
            {
                await transaction.RollbackAsync();
                return false;
            }

            await transaction.CommitAsync();
            return true;
        }
        catch(Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task AddImageAsync(Image image)
    {
        await _dataContext.Images.AddAsync(image);
    }

    public async Task<Image> GetImageByIdAsync(int imageId)
    {
        return await _dataContext.Images.SingleOrDefaultAsync(i => i.Id == imageId);
    }

    public void RemoveImage(Image image)
    {
        _dataContext.Images.Remove(image);
    }
}