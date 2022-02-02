namespace API.Entities;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime DateSent { get; set; }
    public DateTime DateRead { get; set; }

    public AppUser Sender { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; }
    public AppUser Recipient { get; set; }
    public int RecipientId { get; set; }
    public string RecipientName { get; set; }
}