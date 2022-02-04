using API.DTOs.User;
using API.Entities;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<AppUser> GetUserByUserNameAsync(string userName);
    Task<PersonalDataDto> GetPersonalDataByUserNameAsync(string userName);
    void UpdatePersonalData(AppUser appUser);
    Task<bool> SaveAllAsync();
}