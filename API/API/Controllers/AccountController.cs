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
        var registrationData = _stegomasterService.ProcessRequest(stegomasterRequest.Data);
        var registerDto = new RegisterDto()
        {
            Name = registrationData[0],
            PhoneNumber = registrationData[1],
            Email = registrationData[2],
            Password = registrationData[3],
        };

        if (await _unitOfWork.UserRepository.IsExistUserByUserNameAsync(registerDto.PhoneNumber.Replace("+", "")))
            return BadRequest("Введенный номер телефона уже зарегистрирован");
        
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

        var registerResponseData = _stegomasterService.ProcessResponse(new List<string>()
        {
            user.UserName, user.Name, user.PhoneNumber, user.Email, await _tokenService.CreateToken(user),
        });

        return new StegomasterResponse() { Data = registerResponseData };
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<StegomasterResponse>> SignIn(StegomasterRequest stegomasterRequest)
    {
        var signInData = _stegomasterService.ProcessRequest(stegomasterRequest.Data);
        var signInDto = new SignInDto()
        {
            PhoneNumber = signInData[0],
            Password = signInData[1],
        };

        var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(signInDto.PhoneNumber.Replace("+", ""));

        if (user == null)
            return Unauthorized("Введенный телефон не зарегистрирован");

        if (!(await _signInManager.CheckPasswordSignInAsync(user, signInDto.Password, false)).Succeeded)
            return Unauthorized("Неверный пароль");

        var signInResponseData = _stegomasterService.ProcessResponse(new List<string>()
        {
            user.UserName, user.Name, user.PhoneNumber, user.Email, await _tokenService.CreateToken(user),
        });

        return new StegomasterResponse() { Data = signInResponseData };
    }
}