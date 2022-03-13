namespace API.SignalR;

public class PresenceTracker
{
    private static readonly Dictionary<int, List<string>> OnlineUsers = new();


    public Task<bool> UserConnected(int userId, string connectionId)
    {
        bool isOnline = false;
        lock (OnlineUsers)
        {
            if (OnlineUsers.ContainsKey(userId))
            {
                OnlineUsers[userId].Add(connectionId);
            }
            else
            {
                OnlineUsers.Add(userId, new List<string> { connectionId });
                isOnline = true;
            }
        }

        return Task.FromResult(isOnline);
    }

    public Task<bool> UserDisconnected(int userId, string connectionId)
    {
        bool isOffline = false;
        lock (OnlineUsers)
        {
            if (!OnlineUsers.ContainsKey(userId))
                return Task.FromResult(isOffline);

            OnlineUsers[userId].Remove(connectionId);

            if (OnlineUsers[userId].Count == 0)
            {
                OnlineUsers.Remove(userId);
                isOffline = true;
            }
        }

        return Task.FromResult(isOffline);
    }

    public Task<int[]> GetOnlineUsers()
    {
        lock(OnlineUsers)
        {
            return Task.FromResult(OnlineUsers
                .OrderBy(ou => ou.Key)
                .Select(ou => ou.Key)
                .ToArray());
        }
    }

    public Task<List<string>> GetConnectionsForUser(int userId)
    {
        lock(OnlineUsers)
        {
            return Task.FromResult(OnlineUsers.GetValueOrDefault(userId));
        }
    }
}