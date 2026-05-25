using IndustryPulse.Application.DTOs.Ordens;
using IndustryPulse.Application.Services.Interfaces;
using IndustryPulse.Domain.Entities;
using IndustryPulse.Domain.Interfaces.Repositories;

namespace IndustryPulse.Application.Services;

public class OrdemService : IOrdemService
{
    private readonly IOrdemRepository _ordemRepository;

    public OrdemService(IOrdemRepository ordemRepository)
    {
        _ordemRepository = ordemRepository;
    }

    public async Task<IEnumerable<OrdemResponseDTO>> BuscarTodasAsync()
    {
        var ordens = await _ordemRepository.BuscarTodosAsync();
        return ordens.Select(ToDTO);
    }

    public async Task<OrdemResponseDTO?> BuscarPorIdAsync(int id)
    {
        var ordem = await _ordemRepository.BuscarPorIdAsync(id);
        return ordem == null ? null : ToDTO(ordem);
    }

    public async Task<OrdemResponseDTO> CriarAsync(CriarOrdemDTO dto)
    {
        var ano = DateTime.Now.Year;
        var total = await _ordemRepository.ContarPorAnoAsync(ano);

        var ordem = new OrdemProducao
    {
        Numero = $"OP-{ano}-{(total + 1):D4}",
        ProdutoId = dto.ProdutoId,
        LinhaProducaoId = dto.LinhaProducaoId,
        QuantidadePlanejada = dto.QuantidadePlanejada,
        DataPrevisao = DateTime.SpecifyKind(dto.DataPrevisao, DateTimeKind.Utc),
        Observacao = dto.Observacao,
        DataAbertura = DateTime.UtcNow
    };

        await _ordemRepository.CriarAsync(ordem);
        return ToDTO(ordem);
    }

    public async Task AtualizarStatusAsync(int id, AtualizarStatusDTO dto)
    {
        var ordem = await _ordemRepository.BuscarPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Ordem {id} não encontrada");

        if (!ordem.PodeTransicionarPara(dto.NovoStatus))
            throw new InvalidOperationException(
                $"Transição de {ordem.Status} para {dto.NovoStatus} não permitida");

        ordem.Status = dto.NovoStatus;
        ordem.AtualizadoEm = DateTime.UtcNow;

        if (dto.NovoStatus == Domain.Enums.StatusOrdem.Finalizada)
            ordem.DataFinalizacao = DateTime.UtcNow;

        await _ordemRepository.AtualizarAsync(ordem);
    }

    public async Task RegistrarParadaAsync(int id, RegistrarParadaDTO dto)
    {
        var ordem = await _ordemRepository.BuscarPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Ordem {id} não encontrada");

        var parada = new ParadaProducao
        {
            OrdemProducaoId = id,
            Motivo = dto.Motivo,
            Descricao = dto.Descricao,
            Inicio = DateTime.UtcNow
        };

        ordem.Paradas.Add(parada);
        await _ordemRepository.AtualizarAsync(ordem);
    }

    public async Task FecharParadaAsync(int ordemId, int paradaId)
    {
        var ordem = await _ordemRepository.BuscarPorIdAsync(ordemId)
            ?? throw new KeyNotFoundException($"Ordem {ordemId} não encontrada");

        var parada = ordem.Paradas.FirstOrDefault(p => p.Id == paradaId)
            ?? throw new KeyNotFoundException($"Parada {paradaId} não encontrada");

        parada.Fechar();
        await _ordemRepository.AtualizarAsync(ordem);
    }

    private static OrdemResponseDTO ToDTO(OrdemProducao o) => new(
        o.Id,
        o.Numero,
        o.Status,
        o.Produto?.Descricao ?? string.Empty,
        o.LinhaProducao?.Nome ?? string.Empty,
        o.QuantidadePlanejada,
        o.QuantidadeProduzida,
        o.CalcularPercentualConclusao(),
        o.EstaAtrasada(),
        o.DataAbertura,
        o.DataPrevisao,
        o.DataFinalizacao,
        o.Observacao
    );
}