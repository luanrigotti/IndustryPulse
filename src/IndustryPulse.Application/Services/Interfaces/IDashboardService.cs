using IndustryPulse.Application.DTOs.Dashboard;

namespace IndustryPulse.Application.Services.Interfaces;

public interface IDashboardService
{
    Task<KpiGeralDTO> ObterKpisAsync(DateTime inicio, DateTime fim);
    Task<IEnumerable<EficienciaLinhaDTO>> ObterEficienciaPorLinhaAsync(DateTime inicio, DateTime fim);
    Task<IEnumerable<ProducaoDiariaDTO>> ObterProducaoDiariaAsync(int dias);
    Task<IEnumerable<ParetoParadaDTO>> ObterParetoParadasAsync(DateTime inicio, DateTime fim);
}