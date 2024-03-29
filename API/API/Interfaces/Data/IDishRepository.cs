﻿using API.DTOs;
using API.DTOs.Admin;
using API.Entities;
using API.Helpers.Paginator;
using API.Helpers.QueryParams;

namespace API.Interfaces.Data;

public interface IDishRepository
{
    Task<bool> AddDishAsync(DishAddDto dishAddDto);
    Task<Image> GetImageByIdAsync(int imageId);
    void RemoveImage(Image image);
    Task<PagedList<DishAdminListDto>> GetDishesAdminListAsync(DishAdminListParam dishAdminListParam);
    Task<bool> AnyDishByIdAsync(int id);
    Task<DishDto> GetFullDishByIdAsync(int id);
    Task<Dish> GetDishByIdAsync(int id);
    void UpdateDish(Dish dish);
    Task<List<ImageDto>> AddImagesAsync(List<IFormFile> imageFiles, int dishId);
    Task<bool> UpdateDishIngredientsAsync(List<DishAddIngredientDto> ingredients, int dishId);
    Task<PagedList<DishDto>> GetDishesUserListAsync(DishUserListParam dishUserListParam);
}