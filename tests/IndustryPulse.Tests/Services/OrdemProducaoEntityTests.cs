using FluentAssertions;
using IndustryPulse.Domain.Entities;
using IndustryPulse.Domain.Enums;

namespace IndustryPulse.Tests.Services;

public class OrdemProducaoEntityTests
{
    [Theory]
    [InlineData(StatusOrdem.Aberta, StatusOrdem.EmAndamento, true)]
    [InlineData(StatusOrdem.Aberta, StatusOrdem.Cancelada, true)]
    [InlineData(StatusOrdem.Aberta, StatusOrdem.Finalizada, false)]
    [InlineData(StatusOrdem.EmAndamento, StatusOrdem.Finalizada, true)]
    [InlineData(StatusOrdem.EmAndamento, StatusOrdem.Cancelada, false)]
    [InlineData(StatusOrdem.Finalizada, StatusOrdem.Aberta, false)]
    public void PodeTransicionarPara_DeveValidarTransicoesCorretamente(
        StatusOrdem statusAtual,
        StatusOrdem novoStatus,
        bool esperado)
    {
        // Arrange
        var ordem = new OrdemProducao
        {
            Status = statusAtual,
            DataPrevisao = DateTime.UtcNow.AddDays(7),
            DataAbertura = DateTime.UtcNow
        };

        // Act
        var resultado = ordem.PodeTransicionarPara(novoStatus);

        // Assert
        resultado.Should().Be(esperado);
    }

    [Fact]
    public void CalcularPercentualConclusao_DeveRetornarZeroQuandoPlanejadoZero()
    {
        var ordem = new OrdemProducao
        {
            QuantidadePlanejada = 0,
            QuantidadeProduzida = 0,
            DataPrevisao = DateTime.UtcNow.AddDays(7),
            DataAbertura = DateTime.UtcNow
        };

        ordem.CalcularPercentualConclusao().Should().Be(0);
    }

    [Fact]
    public void CalcularPercentualConclusao_DeveCalcularCorretamente()
    {
        var ordem = new OrdemProducao
        {
            QuantidadePlanejada = 100,
            QuantidadeProduzida = 75,
            DataPrevisao = DateTime.UtcNow.AddDays(7),
            DataAbertura = DateTime.UtcNow
        };

        ordem.CalcularPercentualConclusao().Should().Be(75);
    }

    [Fact]
    public void EstaAtrasada_DeveRetornarTrueQuandoDataPrevisaoUltrapassada()
    {
        var ordem = new OrdemProducao
        {
            Status = StatusOrdem.EmAndamento,
            DataPrevisao = DateTime.UtcNow.AddDays(-1),
            DataAbertura = DateTime.UtcNow.AddDays(-5)
        };

        ordem.EstaAtrasada().Should().BeTrue();
    }

    [Fact]
    public void EstaAtrasada_DeveRetornarFalseQuandoFinalizada()
    {
        var ordem = new OrdemProducao
        {
            Status = StatusOrdem.Finalizada,
            DataPrevisao = DateTime.UtcNow.AddDays(-1),
            DataAbertura = DateTime.UtcNow.AddDays(-5),
            DataFinalizacao = DateTime.UtcNow
        };

        ordem.EstaAtrasada().Should().BeFalse();
    }
}