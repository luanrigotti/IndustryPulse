namespace IndustryPulse.Domain.Entities;

public class Usuario : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string Perfil { get; set; } = "Operador";
    public bool Ativo { get; set; } = true;
}