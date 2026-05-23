using IndustryPulse.Domain.Enums;

namespace IndustryPulse.Domain.Entities;


public class ParadaProducao : BaseEntity
{
    public MotivoParada Motivo { get; set; }
    public string? Descricao { get; set; }
    public DateTime Inicio { get; set; }
    public DateTime? Fim { get; set; }

    public int? DuracaoMinutos { get; private set; }

    public int OrdemProducaoId { get; set; }
    public OrdemProducao OrdemProducao { get; set; } = null!;

    public void Fechar()
    {
        Fim = DateTime.UtcNow;
        DuracaoMinutos = (int)(Fim.Value - Inicio).TotalMinutes;
        AtualizadoEm = DateTime.UtcNow;
    }
}