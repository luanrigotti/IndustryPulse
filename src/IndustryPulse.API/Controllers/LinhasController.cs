using IndustryPulse.Application.DTOs.Linhas;
using IndustryPulse.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndustryPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LinhasController : ControllerBase
{
    private readonly ILinhaService _service;

    public LinhasController(ILinhaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> BuscarTodas()
        => Ok(await _service.BuscarTodasAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var linha = await _service.BuscarPorIdAsync(id);
        return linha == null ? NotFound() : Ok(linha);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarLinhaDTO dto)
    {
        var linha = await _service.CriarAsync(dto);
        return CreatedAtAction(nameof(BuscarPorId), new { id = linha.Id }, linha);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Desativar(int id)
    {
        await _service.DesativarAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Ativar(int id)
    {
        await _service.AtivarAsync(id);
        return NoContent();
    }
}