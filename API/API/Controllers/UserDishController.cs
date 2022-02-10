using API.DTOs.User;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UserDishController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserDishController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    [HttpPost("add-user-dish")]
    public async Task<ActionResult<int>> AddUserDish(UserDishDto userDishDto)
    {
        var userDish = new UserDish { AppUserId = User.GetUserId() };
        _mapper.Map(userDishDto, userDish);

        await _unitOfWork.UserDishRepository.AddUserDishAsync(userDish);

        if (await _unitOfWork.CompleteAsync())
            return userDish.Id;

        return BadRequest("Ошибка добавления продукта");
    }

    [HttpPost("add-user-dishes")]
    public async Task<ActionResult<List<UserDishDto>>> AddUserDishes(List<UserDishDto> userDishesDto)
    {
        await _unitOfWork.UserDishRepository.AddUserDishesAsync(userDishesDto, User.GetUserId());
        return userDishesDto;
    }

    [HttpGet("get-user-dishes")]
    public async Task<ActionResult<List<UserDishDto>>> GetUserDishes([FromQuery] UserDishParams userDishParams)
    {
        var userDishes = await _unitOfWork.UserDishRepository.GetUserDishesAsync(userDishParams, User.GetUserId());

        if (userDishes == null)
            return BadRequest("Не удалось получить продукты");

        Response.AddPaginationHeader(userDishes.CurrentPage, userDishes.PageSize, userDishes.TotalCount, userDishes.TotalPages);

        return Ok(userDishes);
    }

    [HttpDelete("delete-user-dishes")]
    public async Task<ActionResult> DeleteUserDishes(List<int> userDishIndexes)
    {
        await _unitOfWork.UserDishRepository.DeleteUserDishesAsync(userDishIndexes);

        if (await _unitOfWork.CompleteAsync())
            return Ok();

        if(userDishIndexes.Count > 1)
            return BadRequest("Ошибка удаления продуктов");

        return BadRequest("Ошибка удаления продукта");
    }
}