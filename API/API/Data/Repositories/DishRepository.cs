using API.DTOs;
using API.DTOs.Admin;
using API.Entities;
using API.Helpers.Paginator;
using API.Helpers.QueryParams;
using API.Interfaces;
using API.Interfaces.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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

    public async Task<Image> GetImageByIdAsync(int imageId)
    {
        return await _dataContext.Images.SingleOrDefaultAsync(i => i.Id == imageId);
    }

    public void RemoveImage(Image image)
    {
        _dataContext.Images.Remove(image);
    }

    public async Task<PagedList<DishAdminListDto>> GetDishesAdminListAsync(DishAdminListParam dishAdminListParam)
    {
        var source = _dataContext.Dishes
            .AsQueryable()
            .OrderByDescending(p => p.Name)
            .Include(d => d.Images)
            .ProjectTo<DishAdminListDto>(_mapper.ConfigurationProvider)
            .AsNoTracking();

        return await PagedList<DishAdminListDto>
            .CreateAsync(source, dishAdminListParam.CurrentPage, dishAdminListParam.PageSize);
    }

    public async Task<bool> AnyDishByIdAsync(int id)
    {
        return await _dataContext.Dishes.AnyAsync(d => d.Id == id);
    }

    public async Task<DishDto> GetFullDishByIdAsync(int id)
    {
        var dish = await _dataContext.Dishes
            .Where(d => d.Id == id)
            .Include(d => d.Images)
            .Include(d => d.Ingredients)
            .ThenInclude(di => di.Ingredient)
            .SingleOrDefaultAsync();

        if (dish == null)
            return null;

        var dishDto = new DishDto
        {
            Images = new List<ImageDto>(),
            Ingredients = new List<IngredientDto>()
        };

        _mapper.Map(dish, dishDto);

        foreach(var img in dish.Images)
            dishDto.Images.Add(_mapper.Map<Image, ImageDto>(img));

        foreach(var di in dish.Ingredients)
        {
            var ingredientDto = _mapper.Map<Ingredient, IngredientDto>(di.Ingredient);
            ingredientDto.IngredientWeightPerPortion = di.IngredientWeightPerPortion;

            dishDto.Ingredients.Add(ingredientDto);
        }

        return dishDto;
    }

    public async Task<Dish> GetDishByIdAsync(int id)
    {
        return await _dataContext.Dishes.Where(d => d.Id == id).SingleOrDefaultAsync();
    }

    public void UpdateDish(Dish dish)
    {
        _dataContext.Entry(dish).State = EntityState.Modified;
    }

    public async Task<List<ImageDto>> AddImagesAsync(List<IFormFile> imageFiles, int dishId)
    {
        var images = new List<Image>();
        var imagesDto = new List<ImageDto>();

        foreach (var img in imageFiles)
        {
            var result = await _imageService.AddImageAsync(img);

            if (result.Error != null) 
                return null;

            images.Add(new Image
            {
                DishId = dishId,
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            });
        }

        await _dataContext.Images.AddRangeAsync(images);

        if (!(await _dataContext.SaveChangesAsync() > 0))
            return null;

        foreach (var img in images)
            imagesDto.Add(_mapper.Map<Image, ImageDto>(img));

        return imagesDto;
    }

    public async Task<bool> UpdateDishIngredientsAsync(List<DishAddIngredientDto> ingredients, int dishId)
    {
        using var transaction = await _dataContext.Database.BeginTransactionAsync();
        try
        {
            var dishIngredients = new List<DishIngredient>();
            var dishIngredientsToDelete = await _dataContext.DishIngredients
                .Where(daid => daid.DishId == dishId)
                .ToListAsync();

            _dataContext.DishIngredients.RemoveRange(dishIngredientsToDelete);

            if (!(await _dataContext.SaveChangesAsync() > 0))
            {
                await transaction.RollbackAsync();
                return false;
            }

            foreach (var ingr in ingredients)
            {
                dishIngredients.Add(new DishIngredient
                {
                    IngredientWeightPerPortion = ingr.IngredientWeightPerPortion,
                    DishId = dishId,
                    IngredientId = ingr.Id
                });
            }

            await _dataContext.DishIngredients.AddRangeAsync(dishIngredients);

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
}