using IndustryPulse.Domain.Enums;

namespace IndustryPulse.Application.DTOs.Ordens;

public record RegistrarParadaDTO(
    MotivoParada Motivo,
    string? Descricao
);