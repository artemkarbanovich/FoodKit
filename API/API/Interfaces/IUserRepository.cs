using API.DTOs.User;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<PersonalDataDto> GetPersonalDataByUserName(string userName);
}