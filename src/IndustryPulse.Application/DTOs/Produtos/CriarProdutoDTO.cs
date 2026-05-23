namespace IndustryPulse.Application.DTOs.Produtos;

public record CriarProdutoDTO(
    string Codigo,
    string Descricao,
    string UnidadeMedida,
    decimal TempoProducaoMinutos
);