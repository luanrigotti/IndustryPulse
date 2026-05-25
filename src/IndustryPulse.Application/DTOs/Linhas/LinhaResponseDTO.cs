namespace IndustryPulse.Application.DTOs.Linhas;

public record LinhaResponseDTO(
    int Id,
    string Nome,
    string Descricao,
    decimal CapacidadeHora,
    bool Ativa
);