using Fiap.Banco.API.Data;
using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Models;
using Fiap.Banco.API.Utils;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Banco.API.Services;

public class ClienteService : IClienteService
{
    private readonly AppDbContext _context;

    public ClienteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<PessoaFisica>> CriarPessoaFisicaAsync(ClientePFRequest request, CancellationToken cancellationToken = default)
    {
        var cpf = DocumentoUtils.SomenteNumeros(request.CPF);

        if (!DocumentoUtils.CpfValido(cpf))
        {
            return OperationResult<PessoaFisica>.Fail("CPF inválido.", 400);
        }

        var agenciaExiste = await _context.AgenciaBanco.AnyAsync(x => x.idAgencia == request.idAgencia, cancellationToken);
        if (!agenciaExiste)
        {
            return OperationResult<PessoaFisica>.Fail("Agência inexistente.", 404);
        }

        var duplicado = await _context.ClientesBanco.OfType<PessoaFisica>().AnyAsync(x => x.CPF == cpf, cancellationToken);
        if (duplicado)
        {
            return OperationResult<PessoaFisica>.Fail("CPF duplicado.", 409);
        }

        var cliente = new PessoaFisica
        {
            nmCliente = request.nmCliente.Trim(),
            CPF = cpf,
            DataNascimento = request.DataNascimento.Date,
            idAgencia = request.idAgencia
        };

        _context.ClientesBanco.Add(cliente);
        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult<PessoaFisica>.Ok(cliente, 201);
    }

    public async Task<OperationResult<PessoaJuridica>> CriarPessoaJuridicaAsync(ClientePJRequest request, CancellationToken cancellationToken = default)
    {
        var cnpj = DocumentoUtils.SomenteNumeros(request.CNPJ);

        if (!DocumentoUtils.CnpjValido(cnpj))
        {
            return OperationResult<PessoaJuridica>.Fail("CNPJ inválido.", 400);
        }

        var agenciaExiste = await _context.AgenciaBanco.AnyAsync(x => x.idAgencia == request.idAgencia, cancellationToken);
        if (!agenciaExiste)
        {
            return OperationResult<PessoaJuridica>.Fail("Agência inexistente.", 404);
        }

        var duplicado = await _context.ClientesBanco.OfType<PessoaJuridica>().AnyAsync(x => x.CNPJ == cnpj, cancellationToken);
        if (duplicado)
        {
            return OperationResult<PessoaJuridica>.Fail("CNPJ duplicado.", 409);
        }

        var cliente = new PessoaJuridica
        {
            nmCliente = request.nmCliente.Trim(),
            CNPJ = cnpj,
            RazaoSocial = request.RazaoSocial.Trim(),
            idAgencia = request.idAgencia
        };

        _context.ClientesBanco.Add(cliente);
        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult<PessoaJuridica>.Ok(cliente, 201);
    }

    public async Task<OperationResult<Cliente>> ObterPorIdAsync(int idCliente, CancellationToken cancellationToken = default)
    {
        var cliente = await _context.ClientesBanco
            .Include(x => x.Agencia)
            .Include(x => x.Contratacoes)
                .ThenInclude(x => x.Produto)
            .FirstOrDefaultAsync(x => x.idCliente == idCliente, cancellationToken);

        return cliente is null
            ? OperationResult<Cliente>.Fail("Cliente não encontrado.", 404)
            : OperationResult<Cliente>.Ok(cliente);
    }
}
