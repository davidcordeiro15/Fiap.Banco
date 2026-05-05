using Fiap.Banco.API.DTOs;

namespace Fiap.Banco.API.Messaging;

public interface IRabbitMqPublisher
{
    Task PublicarContratacaoAsync(ContratacaoMessage message, CancellationToken cancellationToken = default);
}
