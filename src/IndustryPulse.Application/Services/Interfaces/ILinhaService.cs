using IndustryPulse.Application.DTOs.Linhas;

namespace IndustryPulse.Application.Services.Interfaces;

public interface ILinhaService
{
    Task<IEnumerable<LinhaResponseDTO>> BuscarTodasAsync();
    Task<LinhaResponseDTO?> BuscarPorIdAsync(int id);
    Task<LinhaResponseDTO> CriarAsync(CriarLinhaDTO dto);
    Task DesativarAsync(int id);
}