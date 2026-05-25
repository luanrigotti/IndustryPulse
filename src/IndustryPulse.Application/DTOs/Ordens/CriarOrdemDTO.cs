namespace IndustryPulse.Application.DTOs.Ordens;

public record CriarOrdemDTO(
    int ProdutoId,
    int LinhaProducaoId,
    decimal QuantidadePlanejada,
    DateTime DataPrevisao,
    string? Observacao
);