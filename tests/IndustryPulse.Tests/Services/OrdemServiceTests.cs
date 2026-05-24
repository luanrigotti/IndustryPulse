using FluentAssertions;
using IndustryPulse.Application.DTOs.Ordens;
using IndustryPulse.Application.Services;
using IndustryPulse.Domain.Entities;
using IndustryPulse.Domain.Enums;
using IndustryPulse.Domain.Interfaces.Repositories;
using Moq;

namespace IndustryPulse.Tests.Services;

public class OrdemServiceTests
{
    private readonly Mock<IOrdemRepository> _ordemRepositoryMock;
    private readonly OrdemService _service;

    public OrdemServiceTests()
    {
        _ordemRepositoryMock = new Mock<IOrdemRepository>();
        _service = new OrdemService(_ordemRepositoryMock.Object);
    }

    [Fact]
    public async Task CriarAsync_DeveRetornarOrdemComNumeroGerado()
    {
        // Arrange
        var dto = new CriarOrdemDTO(
            ProdutoId: 1,
            LinhaProducaoId: 1,
            QuantidadePlanejada: 100,
            DataPrevisao: DateTime.UtcNow.AddDays(7),
            Observacao: null
        );

        _ordemRepositoryMock
            .Setup(r => r.ContarPorAnoAsync(It.IsAny<int>()))
            .ReturnsAsync(0);

        _ordemRepositoryMock
            .Setup(r => r.CriarAsync(It.IsAny<OrdemProducao>()))
            .ReturnsAsync((OrdemProducao o) => o);

        // Act
        var resultado = await _service.CriarAsync(dto);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Numero.Should().StartWith("OP-");
        resultado.Status.Should().Be(StatusOrdem.Aberta);
        resultado.QuantidadePlanejada.Should().Be(100);
    }

    [Fact]
    public async Task CriarAsync_DeveGerarNumeroSequencial()
    {
        // Arrange
        var dto = new CriarOrdemDTO(1, 1, 50, DateTime.UtcNow.AddDays(5), null);
        var ano = DateTime.Now.Year;

        _ordemRepositoryMock
            .Setup(r => r.ContarPorAnoAsync(ano))
            .ReturnsAsync(5);

        _ordemRepositoryMock
            .Setup(r => r.CriarAsync(It.IsAny<OrdemProducao>()))
            .ReturnsAsync((OrdemProducao o) => o);

        // Act
        var resultado = await _service.CriarAsync(dto);

        // Assert
        resultado.Numero.Should().Be($"OP-{ano}-0006");
    }

    [Fact]
    public async Task AtualizarStatusAsync_DeveAtualizarStatusValido()
    {
        // Arrange
        var ordem = new OrdemProducao
        {
            Id = 1,
            Numero = "OP-2025-0001",
            Status = StatusOrdem.Aberta,
            QuantidadePlanejada = 100,
            DataPrevisao = DateTime.UtcNow.AddDays(7),
            DataAbertura = DateTime.UtcNow
        };

        var dto = new AtualizarStatusDTO(StatusOrdem.EmAndamento, null);

        _ordemRepositoryMock
            .Setup(r => r.BuscarPorIdAsync(1))
            .ReturnsAsync(ordem);

        _ordemRepositoryMock
            .Setup(r => r.AtualizarAsync(It.IsAny<OrdemProducao>()))
            .ReturnsAsync((OrdemProducao o) => o);

        // Act
        await _service.AtualizarStatusAsync(1, dto);

        // Assert
        _ordemRepositoryMock.Verify(
            r => r.AtualizarAsync(It.Is<OrdemProducao>(
                o => o.Status == StatusOrdem.EmAndamento)), Times.Once);
    }

    [Fact]
    public async Task AtualizarStatusAsync_DeveLancarExcecaoParaTransicaoInvalida()
    {
        // Arrange
        var ordem = new OrdemProducao
        {
            Id = 1,
            Status = StatusOrdem.Aberta,
            DataPrevisao = DateTime.UtcNow.AddDays(7),
            DataAbertura = DateTime.UtcNow
        };

        var dto = new AtualizarStatusDTO(StatusOrdem.Finalizada, null);

        _ordemRepositoryMock
            .Setup(r => r.BuscarPorIdAsync(1))
            .ReturnsAsync(ordem);

        // Act
        var act = async () => await _service.AtualizarStatusAsync(1, dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*não permitida*");
    }

    [Fact]
    public async Task AtualizarStatusAsync_DeveLancarExcecaoQuandoOrdemNaoEncontrada()
    {
        // Arrange
        _ordemRepositoryMock
            .Setup(r => r.BuscarPorIdAsync(99))
            .ReturnsAsync((OrdemProducao?)null);

        var dto = new AtualizarStatusDTO(StatusOrdem.EmAndamento, null);

        // Act
        var act = async () => await _service.AtualizarStatusAsync(99, dto);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*99*");
    }

    [Fact]
    public async Task BuscarPorIdAsync_DeveRetornarNullQuandoNaoEncontrado()
    {
        // Arrange
        _ordemRepositoryMock
            .Setup(r => r.BuscarPorIdAsync(99))
            .ReturnsAsync((OrdemProducao?)null);

        // Act
        var resultado = await _service.BuscarPorIdAsync(99);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task BuscarTodasAsync_DeveRetornarListaMapeada()
    {
        // Arrange
        var ordens = new List<OrdemProducao>
        {
            new() {
                Id = 1,
                Numero = "OP-2025-0001",
                Status = StatusOrdem.Aberta,
                QuantidadePlanejada = 100,
                QuantidadeProduzida = 0,
                DataAbertura = DateTime.UtcNow,
                DataPrevisao = DateTime.UtcNow.AddDays(7),
                Produto = new Produto { Descricao = "Produto A" },
                LinhaProducao = new LinhaProducao { Nome = "Linha A" }
            }
        };

        _ordemRepositoryMock
            .Setup(r => r.BuscarTodosAsync())
            .ReturnsAsync(ordens);

        // Act
        var resultado = await _service.BuscarTodasAsync();

        // Assert
        resultado.Should().HaveCount(1);
        resultado.First().Numero.Should().Be("OP-2025-0001");
        resultado.First().NomeProduto.Should().Be("Produto A");
    }
}