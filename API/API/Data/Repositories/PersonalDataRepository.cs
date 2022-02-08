using API.DTOs.User;
using API.Entities;
using API.Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class PersonalDataRepository : IPersonalDataRepository
{
    private readonly DataContext _dataContext;

    public PersonalDataRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }


    public async Task<PersonalDataDto> GetPersonalDataByUserNameAsync(string userName)
    {
        var user = await _dataContext.Users.SingleOrDefaultAsync(au => au.UserName == userName);

        if (user == null) return null;

        return new PersonalDataDto
        {
            UserName = user.UserName,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            RegistrationDate = user.RegistrationDate,
            DateOfBirth = user.DateOfBirth.HasValue ? user.DateOfBirth.Value : null,
            Gender = user.Gender.HasValue ? user.Gender : null,
            BodyWeight = user.BodyWeight.HasValue ? user.BodyWeight : null,
            BodyHeight = user.BodyHeight.HasValue ? user.BodyHeight : null,
            PhysicalActivityCoefficient = user.PhysicalActivityCoefficient.HasValue ? user.PhysicalActivityCoefficient : null
        };
    }

    public void UpdatePersonalData(AppUser appUser)
    {
        _dataContext.Entry(appUser).State = EntityState.Modified;
    }
}