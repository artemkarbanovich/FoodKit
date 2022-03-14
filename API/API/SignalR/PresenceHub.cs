using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

[Authorize]
public class PresenceHub : Hub
{
    private readonly PresenceTracker _presenceTracker;

    public PresenceHub(PresenceTracker presenceTracker)
    {
        _presenceTracker = presenceTracker;
    }


    public override async Task OnConnectedAsync()
    {
        if(await _presenceTracker.UserConnected(Context.User.GetUserId(), Context.ConnectionId))
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUserId());

        await Clients.Caller.SendAsync("GetOnlineUsers", await _presenceTracker.GetOnlineUsers());
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if(await _presenceTracker.UserDisconnected(Context.User.GetUserId(), Context.ConnectionId))
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUserId());

        await base.OnDisconnectedAsync(exception);
    }
}