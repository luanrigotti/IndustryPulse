namespace IndustryPulse.Application.DTOs.Dashboard;

public record EficienciaLinhaDTO(
    int LinhaId,
    string NomeLinha,
    decimal CapacidadeHora,
    decimal QuantidadeProduzida,
    decimal EficienciaPercentual,
    int TotalOrdens,
    double TotalMinutosParada
);