using IndustryPulse.Application.DTOs.Dashboard;
using IndustryPulse.Application.Services.Interfaces;
using IndustryPulse.Domain.Enums;
using IndustryPulse.Domain.Interfaces.Repositories;

namespace IndustryPulse.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IOrdemRepository _ordemRepository;
    private readonly ILinhaRepository _linhaRepository;

    public DashboardService(
        IOrdemRepository ordemRepository,
        ILinhaRepository linhaRepository)
    {
        _ordemRepository = ordemRepository;
        _linhaRepository = linhaRepository;
    }

    public async Task<KpiGeralDTO> ObterKpisAsync(DateTime inicio, DateTime fim)
    {
        var ordens = (await _ordemRepository
            .BuscarPorPeriodoAsync(inicio, fim)).ToList();

        var finalizadas = ordens
            .Where(o => o.Status == StatusOrdem.Finalizada).ToList();

        var abertas = ordens
            .Count(o => o.Status == StatusOrdem.Aberta);

        var emAndamento = ordens
            .Count(o => o.Status == StatusOrdem.EmAndamento);

        var canceladas = ordens
            .Count(o => o.Status == StatusOrdem.Cancelada);

        var atrasadas = ordens
            .Count(o => o.EstaAtrasada());

        // Taxa de cumprimento de prazo
        var taxaPrazo = finalizadas.Count == 0 ? 0 :
            Math.Round((decimal)finalizadas
                .Count(o => o.DataFinalizacao <= o.DataPrevisao)
                / finalizadas.Count * 100, 2);

        // Eficiência de produção
        var totalPlanejado = ordens.Sum(o => o.QuantidadePlanejada);
        var totalProduzido = ordens.Sum(o => o.QuantidadeProduzida);
        var eficiencia = totalPlanejado == 0 ? 0 :
            Math.Round(totalProduzido / totalPlanejado * 100, 2);

        // Tempo médio de parada
        var todasParadas = ordens
            .SelectMany(o => o.Paradas)
            .Where(p => p.DuracaoMinutos.HasValue)
            .ToList();

        var tempoMedioParada = todasParadas.Count == 0 ? 0 :
            todasParadas.Average(p => p.DuracaoMinutos!.Value);

        // OEE simplificado
        var disponibilidade = todasParadas.Count == 0 ? 100m :
            Math.Max(0, 100 - (decimal)tempoMedioParada / 480 * 100);

        var oee = Math.Round(disponibilidade * eficiencia / 100, 2);

        return new KpiGeralDTO(
            oee,
            taxaPrazo,
            eficiencia,
            abertas,
            emAndamento,
            finalizadas.Count,
            canceladas,
            atrasadas,
            Math.Round(tempoMedioParada, 2)
        );
    }

    public async Task<IEnumerable<EficienciaLinhaDTO>> ObterEficienciaPorLinhaAsync(
        DateTime inicio, DateTime fim)
    {
        var linhas = await _linhaRepository.BuscarAtivasAsync();
        var resultado = new List<EficienciaLinhaDTO>();

        foreach (var linha in linhas)
        {
            var ordens = (await _ordemRepository
                .BuscarPorLinhaAsync(linha.Id))
                .Where(o => o.DataAbertura >= inicio &&
                            o.DataAbertura <= fim)
                .ToList();

            var produzido = ordens.Sum(o => o.QuantidadeProduzida);
            var planejado = ordens.Sum(o => o.QuantidadePlanejada);

            var eficiencia = planejado == 0 ? 0 :
                Math.Round(produzido / planejado * 100, 2);

            var minutosParada = ordens
                .SelectMany(o => o.Paradas)
                .Where(p => p.DuracaoMinutos.HasValue)
                .Sum(p => p.DuracaoMinutos!.Value);

            resultado.Add(new EficienciaLinhaDTO(
                linha.Id,
                linha.Nome,
                linha.CapacidadeHora,
                produzido,
                eficiencia,
                ordens.Count,
                minutosParada
            ));
        }

        return resultado;
    }

    public async Task<IEnumerable<ProducaoDiariaDTO>> ObterProducaoDiariaAsync(int dias)
    {
        var fim = DateTime.UtcNow.Date;
        var inicio = fim.AddDays(-dias);

        var ordens = (await _ordemRepository
            .BuscarPorPeriodoAsync(inicio, fim)).ToList();

        var resultado = new List<ProducaoDiariaDTO>();

        for (var data = inicio; data <= fim; data = data.AddDays(1))
        {
            var ordensData = ordens
                .Where(o => o.DataAbertura.Date == data.Date)
                .ToList();

            resultado.Add(new ProducaoDiariaDTO(
                data,
                ordensData.Sum(o => o.QuantidadePlanejada),
                ordensData.Sum(o => o.QuantidadeProduzida),
                ordensData.Count
            ));
        }

        return resultado;
    }

    public async Task<IEnumerable<ParetoParadaDTO>> ObterParetoParadasAsync(
        DateTime inicio, DateTime fim)
    {
        var ordens = await _ordemRepository.BuscarPorPeriodoAsync(inicio, fim);

        var paradas = ordens
            .SelectMany(o => o.Paradas)
            .Where(p => p.DuracaoMinutos.HasValue)
            .ToList();

        var totalMinutos = paradas.Sum(p => p.DuracaoMinutos!.Value);

        var agrupado = paradas
            .GroupBy(p => p.Motivo)
            .Select(g => new
            {
                Motivo = g.Key.ToString(),
                TotalMinutos = (double)g.Sum(p => p.DuracaoMinutos!.Value),
                Ocorrencias = g.Count()
            })
            .OrderByDescending(x => x.TotalMinutos)
            .ToList();

        var acumulado = 0m;
        var resultado = new List<ParetoParadaDTO>();

        foreach (var item in agrupado)
        {
            acumulado += totalMinutos == 0 ? 0 :
                Math.Round((decimal)item.TotalMinutos / totalMinutos * 100, 2);

            resultado.Add(new ParetoParadaDTO(
                item.Motivo,
                item.TotalMinutos,
                item.Ocorrencias,
                acumulado
            ));
        }

        return resultado;
    }
}