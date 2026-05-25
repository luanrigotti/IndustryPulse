using IndustryPulse.Application.DTOs.Produtos;
using IndustryPulse.Application.Services.Interfaces;
using IndustryPulse.Domain.Entities;
using IndustryPulse.Domain.Interfaces.Repositories;

namespace IndustryPulse.Application.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _repository;

    public ProdutoService(IProdutoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProdutoResponseDTO>> BuscarTodosAsync()
    {
        var produtos = await _repository.BuscarAtivosAsync();
        return produtos.Select(ToDTO);
    }

    public async Task<ProdutoResponseDTO?> BuscarPorIdAsync(int id)
    {
        var produto = await _repository.BuscarPorIdAsync(id);
        return produto == null ? null : ToDTO(produto);
    }

    public async Task<ProdutoResponseDTO> CriarAsync(CriarProdutoDTO dto)
    {
        var existente = await _repository.BuscarPorCodigoAsync(dto.Codigo);
        if (existente != null)
            throw new InvalidOperationException(
                $"Já existe um produto com o código {dto.Codigo}");

        var produto = new Produto
        {
            Codigo = dto.Codigo,
            Descricao = dto.Descricao,
            UnidadeMedida = dto.UnidadeMedida,
            TempoProducaoMinutos = dto.TempoProducaoMinutos
        };

        await _repository.CriarAsync(produto);
        return ToDTO(produto);
    }

    public async Task DesativarAsync(int id)
    {
        var produto = await _repository.BuscarPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Produto {id} não encontrado");

        produto.Ativo = false;
        produto.AtualizadoEm = DateTime.UtcNow;
        await _repository.AtualizarAsync(produto);
    }

    private static ProdutoResponseDTO ToDTO(Produto p) => new(
        p.Id,
        p.Codigo,
        p.Descricao,
        p.UnidadeMedida,
        p.TempoProducaoMinutos,
        p.Ativo
    );
}