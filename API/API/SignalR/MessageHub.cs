using API.Extensions;
using API.Interfaces.Data;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

public class MessageHub : Hub
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MessageHub(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public override async Task OnConnectedAsync()
    {
        var otherUserId = int.Parse(Context.GetHttpContext().Request.Query["otherUserId"]);
        var groupName = GetGroupName(Context.User.GetUserId(), otherUserId);
        var messages = await _unitOfWork.MessageRepository.GetMessageThread(Context.User.GetUserId(), otherUserId);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    private string GetGroupName(int callerId, int otherId)
    {
        return callerId > otherId ? $"{callerId}-{otherId}" : $"{otherId}-{callerId}";
    }
}