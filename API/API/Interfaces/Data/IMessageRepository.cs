using API.DTOs;
using API.DTOs.Admin;
using API.Entities;

namespace API.Interfaces.Data;

public interface IMessageRepository
{
    void AddMessage(Message message);
    Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId);
    Task<List<MessageAdminListDto>> GetMessageAdminList(int supportManagerId);
    void AddGroup(Group group);
    void RemoveConnection(Connection connection);
    Task<Connection> GetConnection(string connectionId);
    Task<Group> GetMessageGroup(string groupName);
    Task<Group> GetGroupForConnection(string connectionId);
}