using IndustryPulse.Domain.Entities;
using IndustryPulse.Domain.Enums;
using IndustryPulse.Domain.Interfaces.Repositories;
using IndustryPulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IndustryPulse.Infrastructure.Repositories;

public class OrdemRepository : BaseRepository<OrdemProducao>, IOrdemRepository
{
    public OrdemRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<OrdemProducao>> BuscarPorStatusAsync(StatusOrdem status)
        => await _context.OrdensProducao
            .Include(o => o.Produto)
            .Include(o => o.LinhaProducao)
            .Where(o => o.Status == status)
            .ToListAsync();

    public async Task<IEnumerable<OrdemProducao>> BuscarPorPeriodoAsync(
        DateTime inicio, DateTime fim)
        => await _context.OrdensProducao
            .Include(o => o.Produto)
            .Include(o => o.LinhaProducao)
            .Where(o => o.DataAbertura >= inicio && o.DataAbertura <= fim)
            .ToListAsync();

    public async Task<IEnumerable<OrdemProducao>> BuscarPorLinhaAsync(int linhaId)
        => await _context.OrdensProducao
            .Include(o => o.Produto)
            .Include(o => o.LinhaProducao)
            .Where(o => o.LinhaProducaoId == linhaId)
            .ToListAsync();

    public async Task<int> ContarPorAnoAsync(int ano)
        => await _context.OrdensProducao
            .CountAsync(o => o.DataAbertura.Year == ano);
}