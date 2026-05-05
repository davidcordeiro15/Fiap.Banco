using Fiap.Banco.API.Data;
using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Banco.API.Services;

public class AgenciaService : IAgenciaService
{
    private readonly AppDbContext _context;

    public AgenciaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<Agencia>> CriarAsync(AgenciaCreateRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.nmEndereco) || string.IsNullOrWhiteSpace(request.cep))
        {
            return OperationResult<Agencia>.Fail("Endereço e CEP são obrigatórios.", 400);
        }

        var agencia = new Agencia
        {
            nmEndereco = request.nmEndereco.Trim(),
            cep = request.cep.Trim()
        };

        _context.AgenciaBanco.Add(agencia);
        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult<Agencia>.Ok(agencia, 201);
    }

    public async Task<OperationResult<Agencia>> ObterPorIdAsync(int idAgencia, CancellationToken cancellationToken = default)
    {
        var agencia = await _context.AgenciaBanco
            .Include(x => x.Clientes)
            .Include(x => x.Contratacoes)
            .FirstOrDefaultAsync(x => x.idAgencia == idAgencia, cancellationToken);

        return agencia is null
            ? OperationResult<Agencia>.Fail("Agência não encontrada.", 404)
            : OperationResult<Agencia>.Ok(agencia);
    }
}
