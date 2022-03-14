using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class MessageAddDto
{
    [Required] public string Content { get; set; }
    [Required] public int RecipientId { get; set; }
}