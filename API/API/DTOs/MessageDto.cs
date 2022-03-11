﻿namespace API.DTOs;

public class MessageDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime DateSent { get; set; }
    public DateTime? DateRead { get; set; }

    public int SenderId { get; set; }
    public string SenderName { get; set; }
    public int RecipientId { get; set; }
    public string RecipientName { get; set; }
}