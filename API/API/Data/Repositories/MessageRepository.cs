﻿using API.DTOs;
using API.DTOs.Admin;
using API.Entities;
using API.Interfaces.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public MessageRepository(DataContext dataContext, IMapper mapper) 
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    
    public void AddMessage(Message message)
    {
        _dataContext.Messages.Add(message);
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId)
    {
        var messages = await _dataContext.Messages
            .Where(m => m.SenderId == currentUserId
                && m.RecipientId == recipientId
                || m.SenderId == recipientId
                && m.RecipientId == currentUserId)
            .OrderBy(m => m.DateSent)
            .ToListAsync();

        var unreadMessages = messages
            .Where(m => m.DateRead == null && m.RecipientId == currentUserId)
            .ToList();

        if(unreadMessages.Any())
        {
            foreach (var message in unreadMessages)
                message.DateRead = DateTime.UtcNow;

            await _dataContext.SaveChangesAsync();
        }

        return _mapper.Map<IEnumerable<MessageDto>>(messages);
    }

    public async Task<List<MessageAdminListDto>> GetMessageAdminList(int supportManagerId)
    {
        return await _dataContext.Messages
            .GroupBy(m => m.SenderId)
            .Where(g => g.Key != supportManagerId)
            .Select(g => new MessageAdminListDto
            {
                RecipientId = g.Key,
                RecipientName = g.OrderByDescending(e => e.SenderName).First().SenderName
            })
            .ToListAsync();
    }

    public void AddGroup(Group group)
    {
        _dataContext.Groups.Add(group);
    }

    public void RemoveConnection(Connection connection)
    {
        _dataContext.Connections.Remove(connection);
    }

    public async Task<Connection> GetConnection(string connectionId)
    {
        return await _dataContext.Connections.FindAsync(connectionId);
    }

    public async Task<Group> GetMessageGroup(string groupName)
    {
        return await _dataContext.Groups
            .Include(g => g.Connections)
            .FirstOrDefaultAsync(g => g.GroupName == groupName);
    }

    public async Task<Group> GetGroupForConnection(string connectionId)
    {
        return await _dataContext.Groups
            .Include(g => g.Connections)
            .Where(g => g.Connections.Any(c => c.ConnectionId == connectionId))
            .FirstOrDefaultAsync();
    }
}