namespace IndustryPulse.Application.DTOs.Dashboard;

public record KpiGeralDTO(
    decimal OeePercentual,
    decimal TaxaCumprimentoPrazo,
    decimal EficienciaProducao,
    int TotalOrdensAbertas,
    int TotalOrdensEmAndamento,
    int TotalOrdensFinalizadas,
    int TotalOrdensCanceladas,
    int TotalOrdensAtrasadas,
    double TempoMedioParadaMinutos
);