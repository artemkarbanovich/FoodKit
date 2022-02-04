using API.DTOs.User;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UserController : BaseApiController
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    [HttpGet("get-personal-data/{userName}")]
    public async Task<ActionResult<PersonalDataDto>> GetPersonalData(string userName)
    {
        return await _userRepository.GetPersonalDataByUserNameAsync(userName);
    }

    [HttpPut("update-personal-data")]
    public async Task<ActionResult> UpdatePersonalData(PersonalDataDto personalDataDto)
    {
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