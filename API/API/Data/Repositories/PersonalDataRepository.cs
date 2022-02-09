using API.DTOs.User;
using API.Entities;
using API.Interfaces.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class PersonalDataRepository : IPersonalDataRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public PersonalDataRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }


    public async Task<PersonalDataDto> GetPersonalDataByUserNameAsync(string userName)
    {
        var user = await _dataContext.Users.SingleOrDefaultAsync(au => au.UserName == userName);

        if (user == null) return null;

        var personalDataDto = new PersonalDataDto();
        _mapper.Map(user, personalDataDto);

        return personalDataDto;
    }

    public void UpdatePersonalData(AppUser appUser)
    {
        _dataContext.Entry(appUser).State = EntityState.Modified;
    }
}