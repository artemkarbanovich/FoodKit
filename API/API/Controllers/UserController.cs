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
        return await _userRepository.GetPersonalDataByUserName(userName);
    }
}