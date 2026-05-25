namespace IndustryPulse.Application.DTOs.Produtos;

public record ProdutoResponseDTO(
    int Id,
    string Codigo,
    string Descricao,
    string UnidadeMedida,
    decimal TempoProducaoMinutos,
    bool Ativo
);