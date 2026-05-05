using System.Text;
using System.Text.Json;
using Fiap.Banco.API.DTOs;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Fiap.Banco.API.Messaging;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly RabbitMqOptions _options;

    public RabbitMqPublisher(IOptions<RabbitMqOptions> options)
    {
        _options = options.Value;
    }

    public Task PublicarContratacaoAsync(ContratacaoMessage message, CancellationToken cancellationToken = default)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password,
            DispatchConsumersAsync = false
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _options.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(exchange: string.Empty, routingKey: _options.QueueName, basicProperties: properties, body: body);
        return Task.CompletedTask;
    }
}
