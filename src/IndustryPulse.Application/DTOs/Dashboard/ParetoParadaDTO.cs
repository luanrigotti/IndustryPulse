namespace IndustryPulse.Application.DTOs.Dashboard;

public record ParetoParadaDTO(
    string Motivo,
    double TotalMinutos,
    int Ocorrencias,
    decimal PercentualAcumulado
);