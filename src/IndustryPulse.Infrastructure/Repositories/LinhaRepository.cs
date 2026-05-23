using IndustryPulse.Domain.Entities;
using IndustryPulse.Domain.Interfaces.Repositories;
using IndustryPulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IndustryPulse.Infrastructure.Repositories;

public class LinhaRepository : BaseRepository<LinhaProducao>, ILinhaRepository
{
    public LinhaRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<LinhaProducao>> BuscarAtivasAsync()
        => await _context.LinhasProducao
            .Where(l => l.Ativa)
            .ToListAsync();
}