using IndustryPulse.Application.DTOs.Ordens;
using IndustryPulse.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndustryPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdensController : ControllerBase
{
    private readonly IOrdemService _service;

    public OrdensController(IOrdemService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> BuscarTodas()
        => Ok(await _service.BuscarTodasAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var ordem = await _service.BuscarPorIdAsync(id);
        return ordem == null ? NotFound() : Ok(ordem);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarOrdemDTO dto)
    {
        var ordem = await _service.CriarAsync(dto);
        return CreatedAtAction(nameof(BuscarPorId), new { id = ordem.Id }, ordem);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> AtualizarStatus(
        int id, [FromBody] AtualizarStatusDTO dto)
    {
        await _service.AtualizarStatusAsync(id, dto);
        return NoContent();
    }

    [HttpPost("{id}/paradas")]
    public async Task<IActionResult> RegistrarParada(
        int id, [FromBody] RegistrarParadaDTO dto)
    {
        await _service.RegistrarParadaAsync(id, dto);
        return NoContent();
    }

    [HttpPut("{id}/paradas/{paradaId}/fechar")]
    public async Task<IActionResult> FecharParada(int id, int paradaId)
    {
        await _service.FecharParadaAsync(id, paradaId);
        return NoContent();
    }
}