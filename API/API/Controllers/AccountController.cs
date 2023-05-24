using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Interfaces.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stegomaster;
using Stegomaster.DTOs;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IStegomasterService _stegomasterService;
    private readonly IUnitOfWork _unitOfWork;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
        ITokenService tokenService, IStegomasterService stegomasterService, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _stegomasterService = stegomasterService;
        _unitOfWork = unitOfWork;
    }


    [HttpPost("register")]
    public async Task<ActionResult<StegomasterResponse>> Register(StegomasterRequest stegomasterRequest)
    {
        LogInfo($"1. Server accepted register request...");

        var registrationData = _stegomasterService.ProcessRequest(stegomasterRequest.Data);
        var registerDto = new RegisterDto()
        {
            Name = registrationData[0],
            PhoneNumber = registrationData[1],
            Email = registrationData[2],
            Password = registrationData[3],
        };
        LogInfo($"6. User entered next registration data:", $"\t{registerDto.Name}", $"\t{registerDto.PhoneNumber}", $"\t{registerDto.Email}", $"\t{registerDto.Password}");

        if (await _unitOfWork.UserRepository.IsExistUserByUserNameAsync(registerDto.PhoneNumber.Replace("+", "")))
        {
            LogError("Error: phone number is already registered.");
            return BadRequest("Phone number is already registered");
        }

        var user = new AppUser
        {
            UserName = registerDto.PhoneNumber.Replace("+", ""),
            PhoneNumber = registerDto.PhoneNumber,
            Email = registerDto.Email,
            Name = registerDto.Name,
            RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        var userResult = await _userManager.CreateAsync(user, registerDto.Password);
        if (!userResult.Succeeded)
            return BadRequest(userResult.Errors);

        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
            return BadRequest(roleResult.Errors);

        var registerResponseData = new List<string>() { user.UserName, user.Name, user.PhoneNumber, user.Email, await _tokenService.CreateToken(user) };
        LogInfo("7. User has next register response data:", $"\t{registerResponseData[0]}", $"\t{registerResponseData[1]}", $"\t{registerResponseData[2]}", $"\t{registerResponseData[3]}", $"{registerResponseData[4]}");

        LogInfo("8. Operation 'filling the container' with response data is started...");
        var registerResponseContainer = _stegomasterService.ProcessResponse(registerResponseData);

        LogInfo("15. Register response is sent to the client...");
        return new StegomasterResponse() { Data = registerResponseContainer };
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<StegomasterResponse>> SignIn(StegomasterRequest stegomasterRequest)
    {
        LogInfo($"1. Server accepted login request...");

        var signInData = _stegomasterService.ProcessRequest(stegomasterRequest.Data);
        var signInDto = new SignInDto()
        {
            PhoneNumber = signInData[0],
            Password = signInData[1],
        };
        LogInfo($"6. User entered next login data:", $"\t{signInDto.PhoneNumber}", $"\t{signInDto.Password}");

        var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(signInDto.PhoneNumber.Replace("+", ""));
        if (user == null)
        {
            LogError("Error: phone number is not registered.");
            return Unauthorized("Phone number is not registered");
        }

        if (!(await _signInManager.CheckPasswordSignInAsync(user, signInDto.Password, false)).Succeeded)
        {
            LogError("Error: invalid password.");
            return Unauthorized("Invalid password");
        }

        var signInResponseData = new List<string>() { user.UserName, user.Name, user.PhoneNumber, user.Email, await _tokenService.CreateToken(user) };
        LogInfo("7. User has next login response data:", $"\t{signInResponseData[0]}", $"\t{signInResponseData[1]}", $"\t{signInResponseData[2]}", $"\t{signInResponseData[3]}", $"{signInResponseData[4]}");

        LogInfo("8. Operation 'filling the container' with response data is started...");
        var signInResponseContainer = _stegomasterService.ProcessResponse(signInResponseData);

        LogInfo("15. Login response is sent to the client...");
        return new StegomasterResponse() { Data = signInResponseContainer };
    }

    private void LogInfo(string header, params string[] info) => _stegomasterService.LogInfo(header, info);
    private void LogError(string error) => _stegomasterService.LogError(error);
}