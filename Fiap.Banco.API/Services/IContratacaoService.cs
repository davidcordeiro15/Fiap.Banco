using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Messaging;
using Fiap.Banco.API.Models;

namespace Fiap.Banco.API.Services;

public interface IContratacaoService : IContratacaoProcessor
{
    Task<OperationResult<Contratacao>> CriarAsync(ContratacaoCreateRequest request, CancellationToken cancellationToken = default);
    Task<OperationResult<Contratacao>> ObterPorIdAsync(int idContratacao, CancellationToken cancellationToken = default);
}
