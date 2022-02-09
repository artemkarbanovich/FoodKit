using API.DTOs.User;
using API.Interfaces.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class PersonalDataController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PersonalDataController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    [HttpGet("get-personal-data/{userName}")]
    public async Task<ActionResult<PersonalDataDto>> GetPersonalData(string userName)
    {
        var personalData = await _unitOfWork.PersonalDataRepository.GetPersonalDataByUserNameAsync(userName);

        if(personalData == null)
            return BadRequest("Ошибка получения личных данных");

        return personalData;
    }

    [HttpPut("update-personal-data")]
    public async Task<ActionResult> UpdatePersonalData(PersonalDataDto personalDataDto)
    {
        if ((await _unitOfWork.UserRepository.IsExistUserByUserNameAsync(personalDataDto.PhoneNumber.Replace("+", "")))
            && personalDataDto.UserName != personalDataDto.PhoneNumber.Replace("+", ""))
                return BadRequest("Введенный номер телефона уже зарегистрирован");

        var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(personalDataDto.UserName);

        if (user == null)
            return BadRequest("Ошибка обновления личных данных");

        _mapper.Map(personalDataDto, user);

        _unitOfWork.PersonalDataRepository.UpdatePersonalData(user);

        if (await _unitOfWork.CompleteAsync())
            return NoContent();

        return BadRequest("Ошибка обновления личных данных");
    }
}