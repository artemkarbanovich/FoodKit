using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Interfaces.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
        ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }


    [HttpPost("register")]
    public async Task<ActionResult<AccountDto>> Register(RegisterDto registerDto)
    {
        if (await _unitOfWork.UserRepository.IsExistUserByUserNameAsync(registerDto.PhoneNumber.Replace("+", "")))
            return BadRequest("Введенный номер телефона уже зарегистрирован");
        
        var user = new AppUser
        {
            UserName = registerDto.PhoneNumber.Replace("+", ""),
            PhoneNumber = registerDto.PhoneNumber,
            Email = registerDto.Email,
            Name = registerDto.Name,
            RegistrationDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var userResult = await _userManager.CreateAsync(user, registerDto.Password);
        if (!userResult.Succeeded)
            return BadRequest(userResult.Errors);

        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
            return BadRequest(roleResult.Errors);

        return new AccountDto
        {
            UserName = user.UserName,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            Token = await _tokenService.CreateToken(user)
        };
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<AccountDto>> SignIn(SignInDto signInDto)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(signInDto.PhoneNumber.Replace("+", ""));

        if (user == null)
            return Unauthorized("Введенный телефон не зарегистрирован");

        if (!(await _signInManager.CheckPasswordSignInAsync(user, signInDto.Password, false)).Succeeded)
            return Unauthorized("Неверный пароль");

        return new AccountDto
        {
            UserName = user.UserName,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            Token = await _tokenService.CreateToken(user)
        };
    }
}