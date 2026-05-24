using IndustryPulse.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndustryPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    [HttpGet("kpis")]
    public async Task<IActionResult> ObterKpis(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fim)
    {
        var kpis = await _service.ObterKpisAsync(
            DateTime.SpecifyKind(inicio, DateTimeKind.Utc),
            DateTime.SpecifyKind(fim, DateTimeKind.Utc));
        return Ok(kpis);
    }

    [HttpGet("eficiencia-linhas")]
    public async Task<IActionResult> ObterEficienciaLinhas(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fim)
    {
        var eficiencia = await _service.ObterEficienciaPorLinhaAsync(
            DateTime.SpecifyKind(inicio, DateTimeKind.Utc),
            DateTime.SpecifyKind(fim, DateTimeKind.Utc));
        return Ok(eficiencia);
    }

    [HttpGet("producao-diaria")]
    public async Task<IActionResult> ObterProducaoDiaria(
        [FromQuery] int dias = 30)
    {
        var producao = await _service.ObterProducaoDiariaAsync(dias);
        return Ok(producao);
    }

    [HttpGet("pareto-paradas")]
    public async Task<IActionResult> ObterParetoParadas(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fim)
    {
        var pareto = await _service.ObterParetoParadasAsync(
            DateTime.SpecifyKind(inicio, DateTimeKind.Utc),
            DateTime.SpecifyKind(fim, DateTimeKind.Utc));
        return Ok(pareto);
    }
}