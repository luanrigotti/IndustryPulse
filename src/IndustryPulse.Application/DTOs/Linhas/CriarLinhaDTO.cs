namespace IndustryPulse.Application.DTOs.Linhas;

public record CriarLinhaDTO(
    string Nome,
    string Descricao,
    decimal CapacidadeHora
);