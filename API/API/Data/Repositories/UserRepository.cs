using API.Entities;
using API.Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;

    public UserRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }


    public async Task<AppUser> GetUserByUserNameAsync(string userName)
    {
        return await _dataContext.Users.SingleOrDefaultAsync(au => au.UserName == userName);
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await _dataContext.Users.SingleOrDefaultAsync(au => au.Id == id);
    }

    public async Task<bool> IsExistUserByUserNameAsync(string userName)
    {
        return await _dataContext.Users.AnyAsync(au => au.UserName == userName);
    }

    public async Task<bool> IsExistUserByIdAsync(int id)
    {
        return await _dataContext.Users.AnyAsync(au => au.Id == id);
    }
}