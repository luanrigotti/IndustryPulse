namespace IndustryPulse.Domain.Entities;

public class Produto : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string UnidadeMedida { get; set; } = string.Empty;
    public decimal TempoProducaoMinutos { get; set; }
    public bool Ativo { get; set; } = true;

    public ICollection<OrdemProducao> Ordens { get; set; }
        = new List<OrdemProducao>();
}