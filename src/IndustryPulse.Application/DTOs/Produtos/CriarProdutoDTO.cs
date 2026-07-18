namespace IndustryPulse.Application.DTOs.Produtos;

public record CriarProdutoDTO(
    string Descricao,
    string UnidadeMedida,
    decimal TempoProducaoMinutos
);