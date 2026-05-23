namespace IndustryPulse.Domain.Entities;

public class LinhaProducao : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal CapacidadeHora { get; set; }
    public bool Ativa { get; set; } = true;

    public ICollection<OrdemProducao> Ordens { get; set; }
        = new List<OrdemProducao>();
}