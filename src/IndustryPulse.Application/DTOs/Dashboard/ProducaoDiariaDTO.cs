namespace IndustryPulse.Application.DTOs.Dashboard;

public record ProducaoDiariaDTO(
    DateTime Data,
    decimal QuantidadePlanejada,
    decimal QuantidadeProduzida,
    int TotalOrdens
);