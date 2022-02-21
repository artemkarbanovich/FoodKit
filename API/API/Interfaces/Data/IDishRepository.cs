using API.DTOs.Admin;
using API.Entities;

namespace API.Interfaces.Data;

public interface IDishRepository
{
    Task<bool> AddDishAsync(DishAddDto dishAddDto);
    Task AddImageAsync(Image image);
    Task<Image> GetImageByIdAsync(int imageId);
    void RemoveImage(Image image);
}