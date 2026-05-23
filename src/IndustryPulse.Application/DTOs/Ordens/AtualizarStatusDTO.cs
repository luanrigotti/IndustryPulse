using IndustryPulse.Domain.Enums;

namespace IndustryPulse.Application.DTOs.Ordens;

public record AtualizarStatusDTO(
    StatusOrdem NovoStatus,
    string? Observacao
);