using API.DTOs.User;
using API.Extensions;
using API.Helpers;
using API.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UserDishController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public UserDishController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("add-user-dishes")]
    public async Task<ActionResult> AddUserDishes(List<UserDishDto> userDishesDto)
    {
        await _unitOfWork.UserDishRepository.AddUserDishesAsync(userDishesDto, User.GetUserId());

        if (await _unitOfWork.CompleteAsync())
            return Ok();

        if(userDishesDto.Count == 1)
            return BadRequest("Ошибка добавления продукта");

        return BadRequest("Ошибка добавления продуктов");
    }

    [HttpGet("get-user-dishes")]
    public async Task<ActionResult<List<UserDishDto>>> GetUserDishes([FromQuery] UserDishParams userDishParams)
    {
        var userDishes = await _unitOfWork.UserDishRepository.GetUserDishesAsync(userDishParams, User.GetUserId());

        if (userDishes == null)
            return BadRequest("Не удалось получить продукты");

        Response.AddPaginationHeader(userDishes.CurrentPage, userDishes.PageSize, userDishes.TotalCount);

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