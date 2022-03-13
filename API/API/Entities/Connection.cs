using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class Connection
{
    public Connection() { }
    public Connection(string connectionId, int userId)
    {
        ConnectionId = connectionId;
        UserId = userId;
    }

    [Key] public string ConnectionId { get; set; }
    public int UserId { get; set; }
}