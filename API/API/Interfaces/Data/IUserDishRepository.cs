using API.DTOs.User;
using API.Entities;
using API.Helpers;
using API.Helpers.Paginator;

namespace API.Interfaces.Data;

public interface IUserDishRepository
{
    Task AddUserDishAsync(UserDish userDish);
    Task AddUserDishesAsync(List<UserDishDto> userDishesDto, int userId);
    Task<PagedList<UserDishDto>> GetUserDishesAsync(UserDishParams userDishParams, int userId);
    Task DeleteUserDishesAsync(List<int> userDishesIndexes);
}