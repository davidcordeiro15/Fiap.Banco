using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Models;

namespace Fiap.Banco.API.Services;

public interface IAgenciaService
{
    Task<OperationResult<Agencia>> CriarAsync(AgenciaCreateRequest request, CancellationToken cancellationToken = default);
    Task<OperationResult<Agencia>> ObterPorIdAsync(int idAgencia, CancellationToken cancellationToken = default);
}
