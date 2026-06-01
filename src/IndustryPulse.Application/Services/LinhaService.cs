using IndustryPulse.Application.DTOs.Linhas;
using IndustryPulse.Application.Services.Interfaces;
using IndustryPulse.Domain.Entities;
using IndustryPulse.Domain.Interfaces.Repositories;

namespace IndustryPulse.Application.Services;

public class LinhaService : ILinhaService
{
    private readonly ILinhaRepository _repository;

    public LinhaService(ILinhaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<LinhaResponseDTO>> BuscarTodasAsync()
    {
        var linhas = await _repository.BuscarTodosAsync();
        return linhas.Select(ToDTO);
    }

    public async Task<LinhaResponseDTO?> BuscarPorIdAsync(int id)
    {
        var linha = await _repository.BuscarPorIdAsync(id);
        return linha == null ? null : ToDTO(linha);
    }

    public async Task<LinhaResponseDTO> CriarAsync(CriarLinhaDTO dto)
    {
        var linha = new LinhaProducao
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            CapacidadeHora = dto.CapacidadeHora
        };

        await _repository.CriarAsync(linha);
        return ToDTO(linha);
    }

    public async Task DesativarAsync(int id)
    {
        var linha = await _repository.BuscarPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Linha {id} não encontrada");

        linha.Ativa = false;
        linha.AtualizadoEm = DateTime.UtcNow;
        await _repository.AtualizarAsync(linha);
    }

    private static LinhaResponseDTO ToDTO(LinhaProducao l) => new(
        l.Id,
        l.Nome,
        l.Descricao,
        l.CapacidadeHora,
        l.Ativa
    );
}