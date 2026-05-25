using IndustryPulse.Application.DTOs.Produtos;
using IndustryPulse.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndustryPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _service;

    public ProdutosController(IProdutoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> BuscarTodos()
        => Ok(await _service.BuscarTodosAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var produto = await _service.BuscarPorIdAsync(id);
        return produto == null ? NotFound() : Ok(produto);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarProdutoDTO dto)
    {
        var produto = await _service.CriarAsync(dto);
        return CreatedAtAction(nameof(BuscarPorId), new { id = produto.Id }, produto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Desativar(int id)
    {
        await _service.DesativarAsync(id);
        return NoContent();
    }
}