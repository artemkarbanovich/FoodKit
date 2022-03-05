using API.DTOs.User;
using API.Entities;
using API.Extensions;
using API.Helpers.Paginator;
using API.Helpers.QueryParams;
using API.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrderController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;

    public OrderController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
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

    [HttpGet("get-user-orders")]
    public async Task<ActionResult<PagedList<OrderUserGetDto>>> GetUserOrders([FromQuery] OrderParam orderParam)
    {
        var userId = User.GetUserId();

        if (!(await _unitOfWork.UserRepository.IsExistUserByIdAsync(userId)))
            return NotFound("Пользователь не найден");

        var orders = await _unitOfWork.OrderRepository.GetUserOrdersAsync(orderParam, userId);

        Response.AddPaginationHeader(orders.CurrentPage, orders.PageSize, orders.TotalCount);

        return orders;
    }

    [HttpPatch("update-order-status/{id}")]
    public async Task<ActionResult> UpdateOrderStatus(int id, [FromQuery] string status)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
        var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        if (userRole == "User" && status != "canceled")
            return Unauthorized();

        _unitOfWork.OrderRepository.UpdateOrderStatus(id, status);

        if (await _unitOfWork.CompleteAsync())
            return NoContent();

        return BadRequest("Ошибка обновления статуса заказа");
    }

    [HttpPatch("update-evaluation/{id}")]
    public async Task<ActionResult> UpdateEvaluation(int id, [FromQuery] int evaluationValue)
    {
        _unitOfWork.OrderRepository.UpdateEvaluation(id, evaluationValue);

        if (await _unitOfWork.CompleteAsync())
            return NoContent();

        return BadRequest("Ошибка обновления оценки заказа");
    }
}