using API.DTOs;
using API.DTOs.Admin;
using API.Entities;
using API.Helpers.Paginator;
using API.Helpers.QueryParams;

namespace API.Interfaces.Data;

public interface IDishRepository
{
    Task<bool> AddDishAsync(DishAddDto dishAddDto);
    Task AddImageAsync(Image image);
    Task<Image> GetImageByIdAsync(int imageId);
    void RemoveImage(Image image);
    Task<PagedList<DishAdminListDto>> GetDishesAdminListAsync(DishAdminListParam dishAdminListParam);
    Task<bool> AnyDishByIdAsync(int id);
    Task<DishDto> GetDishByIdAsync(int id);
}