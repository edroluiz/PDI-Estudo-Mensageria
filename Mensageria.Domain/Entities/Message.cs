namespace Mensageria.Domain.Entities;

public class Message
{
    public Guid Id { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Message(string content)
    {
        Id = Guid.NewGuid();
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }
}