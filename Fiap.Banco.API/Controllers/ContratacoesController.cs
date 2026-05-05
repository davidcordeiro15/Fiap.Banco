using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Models;
using Fiap.Banco.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Banco.API.Controllers;

[ApiController]
[Route("api/contratacoes")]
public class ContratacoesController : ControllerBase
{
    private readonly IContratacaoService _service;

    public ContratacoesController(IContratacaoService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<Contratacao>> Criar([FromBody] ContratacaoCreateRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CriarAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result.Success ? result.Data : new { erro = result.Error });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Contratacao>> ObterPorId(int id, CancellationToken cancellationToken)
    {
        var result = await _service.ObterPorIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result.Success ? result.Data : new { erro = result.Error });
    }
}
