using API.DTOs.User;
using API.Entities;
using API.Interfaces;
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

    public async Task<PersonalDataDto> GetPersonalDataByUserNameAsync(string userName)
    {
        var user = await _dataContext.Users.SingleOrDefaultAsync(au => au.UserName == userName);

        return new PersonalDataDto
        {
            UserName = user.UserName,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            RegistrationDate = user.RegistrationDate,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender,
            BodyWeight = user.BodyWeight,
            BodyHeight = user.BodyHeight,
            PhysicalActivityCoefficient = user.PhysicalActivityCoefficient
        };
    }

    public void UpdatePersonalData(AppUser appUser)
    {
        _dataContext.Entry(appUser).State = EntityState.Modified;
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _dataContext.SaveChangesAsync() > 0;
    }
}