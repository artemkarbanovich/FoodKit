using API.DTOs.User;
using API.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrderController : BaseApiController
{
    private IUnitOfWork _unitOfWork;

    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("make-order")]
    public async Task<ActionResult> MakeOrder(OrderDto orderDto)
    {
        //TODO: save order to db
        return Ok();
    }
}