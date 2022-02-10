using API.DTOs.User;
using API.Entities;
using API.Helpers;
using API.Helpers.Paginator;
using API.Interfaces.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class UserDishRepository : IUserDishRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UserDishRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }


    public async Task AddUserDishAsync(UserDish userDish)
    {
        await _dataContext.UserDishes.AddAsync(userDish);
    }

    public async Task AddUserDishesAsync(List<UserDishDto> userDishesDto, int userId)
    {
        var userDishes = new List<UserDish>();

        foreach(var udd in userDishesDto)
        {
            var userDish = new UserDish { AppUserId = userId };
            _mapper.Map(udd, userDish);
            userDishes.Add(userDish);
        }

        await _dataContext.AddRangeAsync(userDishes);
        await _dataContext.SaveChangesAsync();

        foreach (var udd_ud in userDishesDto.Zip(userDishes, Tuple.Create))
            udd_ud.Item1.Id = udd_ud.Item2.Id;
    }

    public async Task<PagedList<UserDishDto>> GetUserDishesAsync(UserDishParams userDishParams, int userId)
    {
        var query = _dataContext.UserDishes
            .AsQueryable()
            .Where(ud => ud.AppUserId == userId)
            .Where(ud => ud.Name.Replace(" ", "").Contains(userDishParams.SearchStringByName.Replace(" ", "").ToLower()));
    
        query = userDishParams.SortDishDateDescending switch
        {
            true => query.OrderByDescending(ud => ud.DishDate),
            false => query.OrderBy(ud => ud.DishDate)
        };

        var source = query.ProjectTo<UserDishDto>(_mapper.ConfigurationProvider).AsNoTracking();

        return await PagedList<UserDishDto>
            .CreateAsync(source, userDishParams.CurrentPage, userDishParams.PageSize);
    }

    public async Task DeleteUserDishesAsync(List<int> userDishIndexes)
    {
        var userDishesToDelete = await _dataContext.UserDishes
            .Where(ud => userDishIndexes.Contains(ud.Id))
            .ToListAsync();

        _dataContext.UserDishes.RemoveRange(userDishesToDelete);
    }
}