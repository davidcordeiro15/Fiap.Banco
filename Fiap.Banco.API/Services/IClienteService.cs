using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Models;

namespace Fiap.Banco.API.Services;

public interface IClienteService
{
    Task<OperationResult<PessoaFisica>> CriarPessoaFisicaAsync(ClientePFRequest request, CancellationToken cancellationToken = default);
    Task<OperationResult<PessoaJuridica>> CriarPessoaJuridicaAsync(ClientePJRequest request, CancellationToken cancellationToken = default);
    Task<OperationResult<Cliente>> ObterPorIdAsync(int idCliente, CancellationToken cancellationToken = default);
}
