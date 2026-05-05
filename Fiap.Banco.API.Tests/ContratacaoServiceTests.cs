using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Enums;
using Fiap.Banco.API.Models;
using Fiap.Banco.API.Services;
using Fiap.Banco.API.Tests.TestHelpers;
using Xunit;

namespace Fiap.Banco.API.Tests;

public class ContratacaoServiceTests
{
    [Fact]
    public async Task DeveCriarContratacaoValida()
    {
        using var context = TestDbContextFactory.Create();
        context.AgenciaBanco.Add(new Agencia { nmEndereco = "Rua A", cep = "80000000" });
        context.ClientesBanco.Add(new PessoaFisica { nmCliente = "João", CPF = "12345678909", DataNascimento = new DateTime(1990, 1, 1), idAgencia = 1 });
        await context.SaveChangesAsync();

        var publisher = new FakePublisher();
        var service = new ContratacaoService(context, publisher);
        var result = await service.CriarAsync(new ContratacaoCreateRequest(1, TipoProduto.Emprestimo, 10000m, 12, null, null, null, null));

        Assert.True(result.Success);
        Assert.Equal(201, result.StatusCode);
        Assert.Single(publisher.Messages);
        Assert.Equal(StatusContratacao.PENDENTE, result.Data!.Status);
    }

    [Fact]
    public async Task DeveReprovarContratacaoInvalidaPelaRegraDoProduto()
    {
        using var context = TestDbContextFactory.Create();
        context.AgenciaBanco.Add(new Agencia { nmEndereco = "Rua A", cep = "80000000" });
        context.ClientesBanco.Add(new PessoaFisica { nmCliente = "João", CPF = "12345678909", DataNascimento = new DateTime(2010, 1, 1), idAgencia = 1 });
        await context.SaveChangesAsync();

        var publisher = new FakePublisher();
        var service = new ContratacaoService(context, publisher);
        var result = await service.CriarAsync(new ContratacaoCreateRequest(1, TipoProduto.Emprestimo, 10000m, 12, null, null, null, null));

        Assert.True(result.Success);
        Assert.Single(publisher.Messages);

        await service.ProcessarAsync(new ContratacaoMessage(result.Data!.idContratacao, 0));
        var obtida = context.Contratacoes.First();
        Assert.Equal(StatusContratacao.REPROVADO, obtida.Status);
    }

    [Fact]
    public async Task DeveRetornarErroQuandoClienteForInexistente()
    {
        using var context = TestDbContextFactory.Create();
        var publisher = new FakePublisher();
        var service = new ContratacaoService(context, publisher);

        var result = await service.CriarAsync(new ContratacaoCreateRequest(999, TipoProduto.Emprestimo, 10000m, 12, null, null, null, null));

        Assert.False(result.Success);
        Assert.Equal(404, result.StatusCode);
        Assert.Empty(publisher.Messages);
    }

    [Fact]
    public async Task DeveConsultarStatus()
    {
        using var context = TestDbContextFactory.Create();
        context.AgenciaBanco.Add(new Agencia { nmEndereco = "Rua A", cep = "80000000" });
        context.ClientesBanco.Add(new PessoaFisica { nmCliente = "João", CPF = "12345678909", DataNascimento = new DateTime(1990, 1, 1), idAgencia = 1 });
        await context.SaveChangesAsync();

        var publisher = new FakePublisher();
        var service = new ContratacaoService(context, publisher);
        var create = await service.CriarAsync(new ContratacaoCreateRequest(1, TipoProduto.Emprestimo, 10000m, 12, null, null, null, null));
        await service.ProcessarAsync(new ContratacaoMessage(create.Data!.idContratacao, 0));

        var consulta = await service.ObterPorIdAsync(create.Data.idContratacao);
        Assert.True(consulta.Success);
        Assert.NotNull(consulta.Data);
    }
}
