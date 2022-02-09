using API.DTOs.User;
using API.Entities;

namespace API.Interfaces.Data;

public interface IPersonalDataRepository
{
    Task<PersonalDataDto> GetPersonalDataByUserNameAsync(string userName);
    void UpdatePersonalData(AppUser appUser);
}