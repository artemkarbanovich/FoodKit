using API.Entities;

namespace API.Interfaces.Data;

public interface IUserRepository
{
    Task<AppUser> GetUserByUserNameAsync(string userName);
    Task<AppUser> GetUserByIdAsync(int id);
    Task<bool> IsExistUserByUserNameAsync(string userName);
    Task<bool> IsExistUserByIdAsync(int id);
}