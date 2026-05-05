using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Models;
using Fiap.Banco.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Banco.API.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;

    public ClientesController(IClienteService service)
    {
        _service = service;
    }

    [HttpPost("pf")]
    public async Task<ActionResult<PessoaFisica>> CriarPF([FromBody] ClientePFRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CriarPessoaFisicaAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result.Success ? result.Data : new { erro = result.Error });
    }

    [HttpPost("pj")]
    public async Task<ActionResult<PessoaJuridica>> CriarPJ([FromBody] ClientePJRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CriarPessoaJuridicaAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result.Success ? result.Data : new { erro = result.Error });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> ObterPorId(int id, CancellationToken cancellationToken)
    {
        var result = await _service.ObterPorIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result.Success ? result.Data : new { erro = result.Error });
    }
}
