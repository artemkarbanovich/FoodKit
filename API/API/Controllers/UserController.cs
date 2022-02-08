using API.DTOs.User;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UserController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<AppUser> _userManager;

    public UserController(IUserRepository userRepository, UserManager<AppUser> userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }


    [HttpGet("get-personal-data/{userName}")]
    public async Task<ActionResult<PersonalDataDto>> GetPersonalData(string userName)
    {
        return await _userRepository.GetPersonalDataByUserNameAsync(userName);
    }

    [HttpPut("update-personal-data")]
    public async Task<ActionResult> UpdatePersonalData(PersonalDataDto personalDataDto)
    {
        if ((await _userManager.Users.AnyAsync(au => au.UserName == personalDataDto.PhoneNumber.Replace("+", ""))) 
            && personalDataDto.UserName != personalDataDto.PhoneNumber.Replace("+", "")) 
                return BadRequest("Аккаунт с введенным номером телефона уже зарегистрирован");

        var user = await _userRepository.GetUserByUserNameAsync(personalDataDto.UserName);

        user.UserName = personalDataDto.PhoneNumber.Replace("+", "");
        user.Name = personalDataDto.Name;
        user.PhoneNumber = personalDataDto.PhoneNumber;
        user.Email = personalDataDto.Email;
        user.DateOfBirth = personalDataDto.DateOfBirth;
        user.Gender = personalDataDto.Gender;
        user.BodyWeight = personalDataDto.BodyWeight;
        user.BodyHeight = personalDataDto.BodyHeight;
        user.PhysicalActivityCoefficient = personalDataDto.PhysicalActivityCoefficient;

        _userRepository.UpdatePersonalData(user);

        if (await _userRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Ошибка обновления личных данных");
    }
}