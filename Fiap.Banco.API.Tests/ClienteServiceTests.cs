using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Models;
using Fiap.Banco.API.Services;
using Fiap.Banco.API.Tests.TestHelpers;
using Xunit;

namespace Fiap.Banco.API.Tests;

public class ClienteServiceTests
{
    [Fact]
    public async Task DeveCadastrarPessoaFisica()
    {
        using var context = TestDbContextFactory.Create();
        context.AgenciaBanco.Add(new Agencia { nmEndereco = "Rua A", cep = "80000000" });
        await context.SaveChangesAsync();

        var service = new ClienteService(context);
        var result = await service.CriarPessoaFisicaAsync(new ClientePFRequest("João", "123.456.789-09", new DateTime(1990, 1, 1), 1));

        Assert.True(result.Success);
        Assert.Equal(201, result.StatusCode);
        Assert.NotNull(result.Data);
        Assert.Equal("12345678909", result.Data!.CPF);
    }

    [Fact]
    public async Task DeveDetectarCPFDuplicado()
    {
        using var context = TestDbContextFactory.Create();
        context.AgenciaBanco.Add(new Agencia { nmEndereco = "Rua A", cep = "80000000" });
        context.ClientesBanco.Add(new PessoaFisica { nmCliente = "A", CPF = "12345678909", DataNascimento = new DateTime(1990, 1, 1), idAgencia = 1 });
        await context.SaveChangesAsync();

        var service = new ClienteService(context);
        var result = await service.CriarPessoaFisicaAsync(new ClientePFRequest("B", "123.456.789-09", new DateTime(1991, 1, 1), 1));

        Assert.False(result.Success);
        Assert.Equal(409, result.StatusCode);
    }

    [Fact]
    public async Task DeveCadastrarPessoaJuridica()
    {
        using var context = TestDbContextFactory.Create();
        context.AgenciaBanco.Add(new Agencia { nmEndereco = "Rua A", cep = "80000000" });
        await context.SaveChangesAsync();

        var service = new ClienteService(context);
        var result = await service.CriarPessoaJuridicaAsync(new ClientePJRequest("Empresa", "12.345.678/0001-90", "Empresa LTDA", 1));

        Assert.True(result.Success);
        Assert.Equal(201, result.StatusCode);
        Assert.Equal("12345678000190", result.Data!.CNPJ);
    }

    [Fact]
    public async Task DeveRetornarErroQuandoAgenciaForInexistente()
    {
        using var context = TestDbContextFactory.Create();
        var service = new ClienteService(context);

        var result = await service.CriarPessoaFisicaAsync(new ClientePFRequest("João", "123.456.789-09", new DateTime(1990, 1, 1), 99));

        Assert.False(result.Success);
        Assert.Equal(404, result.StatusCode);
    }
}
