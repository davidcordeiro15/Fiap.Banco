using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Models;
using Fiap.Banco.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Banco.API.Controllers;

[ApiController]
[Route("api/agencias")]
public class AgenciasController : ControllerBase
{
    private readonly IAgenciaService _service;

    public AgenciasController(IAgenciaService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<Agencia>> Criar([FromBody] AgenciaCreateRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CriarAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result.Success ? result.Data : new { erro = result.Error });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Agencia>> ObterPorId(int id, CancellationToken cancellationToken)
    {
        var result = await _service.ObterPorIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result.Success ? result.Data : new { erro = result.Error });
    }
}
