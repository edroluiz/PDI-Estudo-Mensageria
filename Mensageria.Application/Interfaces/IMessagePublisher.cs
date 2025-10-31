namespace Mensageria.Application.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync(string messageContent);
}