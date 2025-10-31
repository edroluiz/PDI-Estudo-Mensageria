using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mensageria.Consumer.BackgroundServices;

public class MessageConsumerWorker : BackgroundService
{
    private readonly ILogger<MessageConsumerWorker> _logger;
    private readonly ConnectionFactory _factory;
    private const string QueueName = "minha-fila";

    public MessageConsumerWorker(ILogger<MessageConsumerWorker> logger)
    {
        _logger = logger;
        _factory = new ConnectionFactory() { HostName = "localhost" };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await using var connection = await _factory.CreateConnectionAsync(stoppingToken);
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    _logger.LogInformation("[Mensagem Recebida]: {message}", message);

                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar a mensagem recebida.");
                }
            };

            await channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer);

            _logger.LogInformation("Consumer configurado. Aguardando mensagens na fila '{queue}'...", QueueName);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Tarefa cancelada. O worker está parando.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado no worker. O serviço será interrompido.");
        }
    }
}