using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Messaging;

namespace Fiap.Banco.API.Tests.TestHelpers;

public class FakePublisher : IRabbitMqPublisher
{
    public List<ContratacaoMessage> Messages { get; } = new();

    public Task PublicarContratacaoAsync(ContratacaoMessage message, CancellationToken cancellationToken = default)
    {
        Messages.Add(message);
        return Task.CompletedTask;
    }
}
