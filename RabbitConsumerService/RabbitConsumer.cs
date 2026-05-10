using RabbitConsumerService.Handler;
using RabbitConsumerService.Shared.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace RabbitConsumerService
{
    public class RabbitConsumer : BackgroundService
    {
        private readonly ILogger<RabbitConsumer> _logger;
        private readonly ConnectionFactory _factory;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitConsumer(
            ILogger<RabbitConsumer> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            _factory = new ConnectionFactory()
            {
                HostName = "localhost",
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "record",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                using var scope = _scopeFactory.CreateScope();

                var handler = scope.ServiceProvider
                    .GetRequiredService<RecordHandler>();

                try
                {
                    var body = eventArgs.Body.ToArray();

                    var receivedMessage =
                        JsonSerializer.Deserialize<RecordCreatedEvent>(
                            Encoding.UTF8.GetString(body));

                    _logger.LogInformation(
                        "Received message: {@message}",
                        receivedMessage);

                    if (receivedMessage is not null)
                    {
                        await handler.HandleAsync(receivedMessage);

                        await ((AsyncEventingBasicConsumer)sender!)
                            .Channel
                            .BasicAckAsync(
                                eventArgs.DeliveryTag,
                                multiple: false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error processing RabbitMQ message");

                    await channel.BasicNackAsync(
                        eventArgs.DeliveryTag,
                        multiple: false,
                        requeue: true);
                }
            };

            await channel.BasicConsumeAsync(
                queue: "record",
                autoAck: false,
                consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}