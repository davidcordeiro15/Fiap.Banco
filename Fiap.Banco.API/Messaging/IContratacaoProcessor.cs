using Fiap.Banco.API.DTOs;

namespace Fiap.Banco.API.Messaging;

public interface IContratacaoProcessor
{
    Task ProcessarAsync(ContratacaoMessage message, CancellationToken cancellationToken = default);
}
