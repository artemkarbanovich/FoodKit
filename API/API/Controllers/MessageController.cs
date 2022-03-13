using API.DTOs.Admin;
using API.Entities;
using API.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class MessageController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public MessageController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _roleManager = roleManager;
    }


    [HttpGet("get-support-manager-id")]
    public async Task<ActionResult<int>> GetSupportManagerId()
    {
        var adminRole = await _roleManager.FindByNameAsync("Admin");
        var supportManager = await _userManager.Users
            .FirstOrDefaultAsync(ap => ap.AppUserRoles.Any(aur => aur.RoleId == adminRole.Id));

        if (supportManager != null)
            return supportManager.Id;

        return NotFound("Ошибка получения службы поддержки");
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("get-message-admin-list")]
    public async Task<ActionResult<List<MessageAdminListDto>>> GetMessageAdminList()
    {
        var adminRole = await _roleManager.FindByNameAsync("Admin");
        var supportManager = await _userManager.Users
            .FirstOrDefaultAsync(ap => ap.AppUserRoles.Any(aur => aur.RoleId == adminRole.Id));

        if (supportManager == null)
            return BadRequest("Ошибка получения списка сообщений");

        return await _unitOfWork.MessageRepository.GetMessageAdminList(supportManager.Id);
    }
}