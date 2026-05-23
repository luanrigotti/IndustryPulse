using IndustryPulse.Application.DTOs.Ordens;

namespace IndustryPulse.Application.Services.Interfaces;

public interface IOrdemService
{
    Task<IEnumerable<OrdemResponseDTO>> BuscarTodasAsync();
    Task<OrdemResponseDTO?> BuscarPorIdAsync(int id);
    Task<OrdemResponseDTO> CriarAsync(CriarOrdemDTO dto);
    Task AtualizarStatusAsync(int id, AtualizarStatusDTO dto);
    Task RegistrarParadaAsync(int id, RegistrarParadaDTO dto);
    Task FecharParadaAsync(int ordemId, int paradaId);
}