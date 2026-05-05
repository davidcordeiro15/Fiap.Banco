using System.Text;
using System.Text.Json;
using Fiap.Banco.API.DTOs;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Fiap.Banco.API.Messaging;

public class RabbitMqConsumerHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RabbitMqOptions _options;
    private readonly ILogger<RabbitMqConsumerHostedService> _logger;

    public RabbitMqConsumerHostedService(
        IServiceScopeFactory scopeFactory,
        IOptions<RabbitMqOptions> options,
        ILogger<RabbitMqConsumerHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _options.HostName,
                    UserName = _options.UserName,
                    Password = _options.Password,
                    DispatchConsumersAsync = false,
                    AutomaticRecoveryEnabled = true
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare(queue: _options.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (_, ea) =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var processor = scope.ServiceProvider.GetRequiredService<IContratacaoProcessor>();
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var message = JsonSerializer.Deserialize<ContratacaoMessage>(body) ?? throw new InvalidOperationException("Mensagem inválida.");

                    try
                    {
                        processor.ProcessarAsync(message, stoppingToken).GetAwaiter().GetResult();
                        channel.BasicAck(ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao processar contratação {ContratacaoId}", message.ContratacaoId);

                        if (message.Tentativas < 3)
                        {
                            var retry = message with { Tentativas = message.Tentativas + 1 };
                            var retryBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(retry));
                            var props = channel.CreateBasicProperties();
                            props.Persistent = true;
                            channel.BasicPublish(string.Empty, _options.QueueName, props, retryBody);
                        }
                        else
                        {
                            try
                            {
                                processor.ProcessarAsync(message with { Tentativas = message.Tentativas }, stoppingToken).GetAwaiter().GetResult();
                            }
                            catch
                            {
                                // último recurso: registra e segue adiante
                            }
                        }

                        channel.BasicAck(ea.DeliveryTag, multiple: false);
                    }
                };

                channel.BasicConsume(queue: _options.QueueName, autoAck: false, consumer: consumer);
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha no consumidor RabbitMQ. Nova tentativa em alguns segundos.");
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}
