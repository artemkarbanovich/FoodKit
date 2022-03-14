using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces.Data;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

public class MessageHub : Hub
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHubContext<PresenceHub> _presenceHubContext;
    private readonly PresenceTracker _presenceTracker;

    public MessageHub(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager,
        IHubContext<PresenceHub> presenceHubContext, PresenceTracker presenceTracker)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
        _presenceHubContext = presenceHubContext;
        _presenceTracker = presenceTracker;
    }


    public override async Task OnConnectedAsync()
    {
        var otherUserId = int.Parse(Context.GetHttpContext().Request.Query["otherUserId"]);
        var groupName = GetGroupName(Context.User.GetUserId(), otherUserId);
        var messages = await _unitOfWork.MessageRepository.GetMessageThread(Context.User.GetUserId(), otherUserId);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("UpdatedGroup", await AddToGroup(groupName));
        await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var group = await RemoveFromMessageGroup();

        await Clients.Group(group.GroupName).SendAsync("UpdatedGroup", group);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(MessageAddDto messageAddDto)
    {
        var currentUserId = Context.User.GetUserId();

        if (currentUserId == messageAddDto.RecipientId)
            throw new HubException("Вы не можете отправить сообщение сами себе");

        var sender = await _unitOfWork.UserRepository.GetUserByIdAsync(currentUserId);
        var recipient = await _unitOfWork.UserRepository.GetUserByIdAsync(messageAddDto.RecipientId);

        if (sender == null || recipient == null)
            throw new HubException("Пользователь не найден");

        if ((await _userManager.GetRolesAsync(sender)).FirstOrDefault() == "User"
            && (await _userManager.GetRolesAsync(recipient)).FirstOrDefault() != "Admin")
            throw new HubException("Вы можете отправить сообщение только в поддержку");


        var groupName = GetGroupName(sender.Id, recipient.Id);
        var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);
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

        if (group.Connections.Any(c => c.UserId == recipient.Id))
        {
            message.DateRead = DateTime.Now;
        }
        else
        {
            var connections = await _presenceTracker.GetConnectionsForUser(recipient.Id);
            if(connections != null)
            {
                await _presenceHubContext.Clients.Clients(connections)
                    .SendAsync("NewMessageReceived", new { senderName = message.SenderName, content = message.Content });
            }
        }

        _unitOfWork.MessageRepository.AddMessage(message);

        if (await _unitOfWork.CompleteAsync())
        {
            await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<Message, MessageDto>(message));
        }
    }

    private async Task<Group> AddToGroup(string groupName)
    {
        var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);
        var connection = new Connection(Context.ConnectionId, Context.User.GetUserId());

        if (group == null)
        {
            group = new Group(groupName);
            _unitOfWork.MessageRepository.AddGroup(group);
        }

        group.Connections.Add(connection);

        if (await _unitOfWork.CompleteAsync())
            return group;

        throw new HubException("Ошибка подлючения к группе");
    }

    private async Task<Group> RemoveFromMessageGroup()
    {
        var group = await _unitOfWork.MessageRepository.GetGroupForConnection(Context.ConnectionId);
        var connection = group.Connections.FirstOrDefault(g => g.ConnectionId == Context.ConnectionId);

        _unitOfWork.MessageRepository.RemoveConnection(connection);

        if (await _unitOfWork.CompleteAsync())
            return group;

        throw new HubException("Ошибка отлючения от группы");
    }

    private static string GetGroupName(int callerId, int otherId)
    {
        return callerId > otherId ? $"{callerId}-{otherId}" : $"{otherId}-{callerId}";
    }
}