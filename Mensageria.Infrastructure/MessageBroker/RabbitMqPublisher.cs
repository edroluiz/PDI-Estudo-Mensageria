using Mensageria.Application.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace Mensageria.Infrastructure.MessageBroker;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly ConnectionFactory _factory;
    private const string QueueName = "minha-fila";

    public RabbitMqPublisher()
    {
        _factory = new ConnectionFactory { HostName = "localhost" };
    }

    public async Task PublishAsync(string messageContent)
    {
        await using var connection = await _factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: QueueName,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

        var body = Encoding.UTF8.GetBytes(messageContent);

        await channel.BasicPublishAsync(exchange: string.Empty,
                                        routingKey: QueueName,
                                        body: body);
    }
}