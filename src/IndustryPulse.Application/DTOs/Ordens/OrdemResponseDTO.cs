using IndustryPulse.Domain.Enums;

namespace IndustryPulse.Application.DTOs.Ordens;

public record OrdemResponseDTO(
    int Id,
    string Numero,
    StatusOrdem Status,
    string NomeProduto,
    string NomeLinhaProducao,
    decimal QuantidadePlanejada,
    decimal QuantidadeProduzida,
    decimal PercentualConclusao,
    bool EstaAtrasada,
    DateTime DataAbertura,
    DateTime DataPrevisao,
    DateTime? DataFinalizacao,
    string? Observacao
);