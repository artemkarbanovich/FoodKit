using API.DTOs.User;
using API.Extensions;
using API.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrderController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("make-order")]
    public async Task<ActionResult> MakeOrder(OrderDto orderDto)
    {
        var userId = User.GetUserId();

        if (!(await _unitOfWork.UserRepository.IsExistUserByIdAsync(userId)))
            return NotFound("Пользователь не найден");

        if (!(await _unitOfWork.OrderRepository.MakeOrderAsync(orderDto, userId)))
            return BadRequest("Ошибка формления заказа");

        return Ok();
    }
}