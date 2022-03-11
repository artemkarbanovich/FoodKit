using API.DTOs;
using API.DTOs.Admin;
using API.Entities;
using API.Extensions;
using API.Interfaces.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class MessageController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public MessageController(IUnitOfWork unitOfWork, IMapper mapper, 
        UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
    }


    [HttpPost("send-message")]
    public async Task<ActionResult<MessageDto>> SendMessage(MessageAddDto messageAddDto)
    {
        var currentUserId = User.GetUserId();

        if (currentUserId == messageAddDto.RecipientId)
            return BadRequest("Вы не можете отправить сообщение сами себе");

        var sender = await _unitOfWork.UserRepository.GetUserByIdAsync(currentUserId);
        var recipient = await _unitOfWork.UserRepository.GetUserByIdAsync(messageAddDto.RecipientId);

        if (sender == null || recipient == null)
            return NotFound();

        if ((await _userManager.GetRolesAsync(sender)).FirstOrDefault() == "User"
            && (await _userManager.GetRolesAsync(recipient)).FirstOrDefault() != "Admin")
                return BadRequest("Вы можете отправить сообщение только в поддержку");

        var message = new Message
        {
            Content = messageAddDto.Content,
            DateSent = DateTime.Now,
            DateRead = null,
            SenderId = sender.Id,
            SenderName = sender.Name,
            RecipientId = recipient.Id,
            RecipientName = recipient.Name
        };

        _unitOfWork.MessageRepository.AddMessage(message);

        if (await _unitOfWork.CompleteAsync())
            return Ok(_mapper.Map<Message, MessageDto>(message));

        return BadRequest("Ошибка отправки сообщения");
    }

    [HttpGet("get-message-thread/{recipientId}")]
    public async Task<ActionResult<List<MessageDto>>> GetMessageThread(int recipientId)
    {
        return Ok(await _unitOfWork.MessageRepository.GetMessageThread(User.GetUserId(), recipientId));
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