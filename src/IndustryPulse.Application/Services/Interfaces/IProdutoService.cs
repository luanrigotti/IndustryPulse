using IndustryPulse.Application.DTOs.Produtos;

namespace IndustryPulse.Application.Services.Interfaces;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoResponseDTO>> BuscarTodosAsync();
    Task<ProdutoResponseDTO?> BuscarPorIdAsync(int id);
    Task<ProdutoResponseDTO> CriarAsync(CriarProdutoDTO dto);
    Task DesativarAsync(int id);
}