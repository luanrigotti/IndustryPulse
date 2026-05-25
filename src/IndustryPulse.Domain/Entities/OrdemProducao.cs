using IndustryPulse.Domain.Enums;

namespace IndustryPulse.Domain.Entities;

public class OrdemProducao : BaseEntity
{
    public string Numero { get; set; } = string.Empty;
    public StatusOrdem Status { get; set; } = StatusOrdem.Aberta;
    public decimal QuantidadePlanejada { get; set; }
    public decimal QuantidadeProduzida { get; set; }
    public DateTime DataAbertura { get; set; } = DateTime.UtcNow;
    public DateTime DataPrevisao { get; set; }
    public DateTime? DataFinalizacao { get; set; }
    public string? Observacao { get; set; }

    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } = null!;

    public int LinhaProducaoId { get; set; }
    public LinhaProducao LinhaProducao { get; set; } = null!;

    public ICollection<ParadaProducao> Paradas { get; set; }
        = new List<ParadaProducao>();

    public bool PodeTransicionarPara(StatusOrdem novoStatus)
    {
        return Status switch
        {
            StatusOrdem.Aberta => novoStatus is
                StatusOrdem.EmAndamento or
                StatusOrdem.Cancelada,

            StatusOrdem.EmAndamento => novoStatus is
                StatusOrdem.Finalizada,

            _ => false
        };
    }
    public decimal CalcularPercentualConclusao()
    {
        if (QuantidadePlanejada == 0) return 0;
        return Math.Round(
            QuantidadeProduzida / QuantidadePlanejada * 100, 2);
    }

    public bool EstaAtrasada()
        => Status != StatusOrdem.Finalizada &&
           Status != StatusOrdem.Cancelada &&
           DateTime.UtcNow > DataPrevisao;
}