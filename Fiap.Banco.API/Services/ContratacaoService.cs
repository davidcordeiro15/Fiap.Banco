using Fiap.Banco.API.Data;
using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Enums;
using Fiap.Banco.API.Messaging;
using Fiap.Banco.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Banco.API.Services;

public class ContratacaoService : IContratacaoService
{
    private readonly AppDbContext _context;
    private readonly IRabbitMqPublisher _publisher;

    public ContratacaoService(AppDbContext context, IRabbitMqPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<OperationResult<Contratacao>> CriarAsync(ContratacaoCreateRequest request, CancellationToken cancellationToken = default)
    {
        var cliente = await _context.ClientesBanco
            .Include(x => x.Agencia)
            .FirstOrDefaultAsync(x => x.idCliente == request.idCliente, cancellationToken);

        if (cliente is null)
        {
            return OperationResult<Contratacao>.Fail("Cliente inexistente.", 404);
        }

        if (cliente.Agencia is null)
        {
            return OperationResult<Contratacao>.Fail("Agência inexistente.", 404);
        }

        var produto = CriarProduto(request);
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync(cancellationToken);

        var contratacao = new Contratacao
        {
            idCliente = cliente.idCliente,
            idAgencia = cliente.idAgencia,
            idProduto = produto.idProduto,
            Produto = produto,
            Cliente = cliente,
            Agencia = cliente.Agencia,
            Status = StatusContratacao.PENDENTE,
            TipoProduto = request.TipoProduto.ToString(),
            Tentativas = 0,
            DataCriacao = DateTime.UtcNow
        };

        _context.Contratacoes.Add(contratacao);
        await _context.SaveChangesAsync(cancellationToken);

        await _publisher.PublicarContratacaoAsync(new ContratacaoMessage(contratacao.idContratacao, 0), cancellationToken);

        return OperationResult<Contratacao>.Ok(contratacao, 201);
    }

    public async Task<OperationResult<Contratacao>> ObterPorIdAsync(int idContratacao, CancellationToken cancellationToken = default)
    {
        var contratacao = await _context.Contratacoes
            .Include(x => x.Cliente)
                .ThenInclude(x => x!.Agencia)
            .Include(x => x.Agencia)
            .Include(x => x.Produto)
            .FirstOrDefaultAsync(x => x.idContratacao == idContratacao, cancellationToken);

        return contratacao is null
            ? OperationResult<Contratacao>.Fail("Contratação não encontrada.", 404)
            : OperationResult<Contratacao>.Ok(contratacao);
    }

    public async Task ProcessarAsync(ContratacaoMessage message, CancellationToken cancellationToken = default)
    {
        var contratacao = await _context.Contratacoes
            .Include(x => x.Cliente)
            .Include(x => x.Produto)
            .FirstOrDefaultAsync(x => x.idContratacao == message.ContratacaoId, cancellationToken)
            ?? throw new InvalidOperationException("Contratação não encontrada.");

        if (contratacao.Status is StatusContratacao.APROVADO or StatusContratacao.REPROVADO)
        {
            return;
        }

        contratacao.Status = StatusContratacao.PROCESSANDO;
        contratacao.Tentativas = message.Tentativas;
        contratacao.DataAtualizacao = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        if (contratacao.Produto is null || contratacao.Cliente is null)
        {
            contratacao.Status = StatusContratacao.REPROVADO;
            contratacao.MensagemProcessamento = "Dados da contratação incompletos.";
            contratacao.DataAtualizacao = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }

        if (contratacao.Produto.PodeSerContratado(contratacao.Cliente, out var motivo))
        {
            contratacao.Status = StatusContratacao.APROVADO;
            contratacao.MensagemProcessamento = "Contratação aprovada.";
        }
        else
        {
            contratacao.Status = StatusContratacao.REPROVADO;
            contratacao.MensagemProcessamento = motivo;
        }

        contratacao.DataAtualizacao = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static Produto CriarProduto(ContratacaoCreateRequest request)
    {
        return request.TipoProduto switch
        {
            TipoProduto.Emprestimo => new Emprestimo
            {
                nmProduto = "Empréstimo",
                Descricao = "Produto de crédito pessoal ou empresarial",
                ValorSolicitado = request.ValorSolicitado,
                Parcelas = request.Parcelas
            },
            TipoProduto.MaquinaDeCartao => new MaquinaDeCartao
            {
                nmProduto = "Máquina de Cartão",
                Descricao = "Solução de captura de pagamentos",
                VolumeMensalEstimado = request.VolumeMensalEstimado ?? 0,
                TaxaPercentual = request.TaxaPercentual ?? 0
            },
            TipoProduto.ReceberSalario => new ReceberSalario
            {
                nmProduto = "Receber Salário",
                Descricao = "Conta salário e portabilidade",
                EmpresaConveniada = request.EmpresaConveniada,
                SalarioMensal = request.SalarioMensal ?? 0
            },
            _ => throw new ArgumentOutOfRangeException(nameof(request.TipoProduto), "Tipo de produto inválido.")
        };
    }
}
